using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common.Enums;
using System.Linq;
using ASTRA.EMSG.Common.Master.Calculators;
using NHibernate.Linq;
using Parameter = ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische.ZustandsspiegelProJahrGrafischeParameter;
using DiagramPo = ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische.ZustandsspiegelProJahrGrafischeDiagramPo;
using TablePo = ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische.ZustandsspiegelProJahrGrafischeTablePo;
using MainPo = ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische.ZustandsspiegelProJahrGrafischePo;


namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische
{
    public interface IZustandsspiegelProJahrGrafischePoProvider : IPoProvider
    { 
    }

    [ReportInfo(AuswertungTyp.W5_2)]
    public class ZustandsspiegelProJahrGrafischePoProvider : EmsgFilterablePoProviderBase<Parameter, ZustandsspiegelProJahrGrafischeDiagramPo>, IZustandsspiegelProJahrGrafischePoProvider
    {
        private readonly IJahresIntervalService jahresIntervalService;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IReportResourceLocator reportResourceLocator;
        private readonly IReportLegendImageService reportLegendImageService;

        private const string FullSubReportName = "ZustandsspiegelProJahrGrafischeFullSubReport";
        private const string PartialSubReportName = "ZustandsspiegelProJahrGrafischePartialSubReport";

        private const string SubReportDefinitionResourceName = "ASTRA.EMSG.Business.Reports.ZustandsspiegelProJahrGrafische.ZustandsspiegelProJahrGrafischeSubReport.rdlc";

        private const int MaxColumnCount = 12;

        public ZustandsspiegelProJahrGrafischePoProvider(
            IJahresIntervalService jahresIntervalService,
            IFiltererFactory filtererFactory,
            ITransactionScopeProvider transactionScopeProvider,
            IReportLocalizationService reportLocalizationService, 
            IReportResourceLocator reportResourceLocator,
            IReportLegendImageService reportLegendImageService)
        {
            this.jahresIntervalService = jahresIntervalService;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.reportLocalizationService = reportLocalizationService;
            this.reportResourceLocator = reportResourceLocator;
            this.reportLegendImageService = reportLegendImageService;

            diagramPos = new Dictionary<int, List<DiagramPo>>();
            tablePos = new Dictionary<int, List<TablePo>>();
        }

        protected override bool IsForClosedErfassungsPeriod(Parameter parameter)
        {
            return ErfassungsPeriodService.GetEntityById(parameter.ErfassungsPeriodIdBis).IsClosed;
        }

        private readonly Dictionary<int, List<DiagramPo>> diagramPos;
        private readonly Dictionary<int, List<TablePo>> tablePos;

        public override void LoadDataSources(Parameter parameter)
        {
            base.LoadDataSources(parameter);
            CalculatePos(parameter);
            HasData = tablePos.Any() && diagramPos.Any();
            SetReportParameters(parameter);
        }
        
        public override IEnumerable<ReportDataSource> DataSources
        {
            get
            {
                var mainPos = Enumerable.Range(0, (tablePos.Count / MaxColumnCount)).Select(i => new MainPo { Page = i }).ToList();
                return new List<ReportDataSource> 
                { 
                    new ReportDataSource(mainPos, ReportDataSourceFactory),
                };
            }
        }

        private void CalculatePos(Parameter parameter)
        {
            List<ErfassungsPeriod> erfassungsPeriodList = ErfassungsPeriodService.GetErfassungsPeriods(parameter.ErfassungsPeriodIdVon, parameter.ErfassungsPeriodIdBis).ToList();

            for (int i = 0; i < erfassungsPeriodList.Count; i++)
            {
                var erfassungsPeriod = erfassungsPeriodList[i];

                var jahresInterval = jahresIntervalService.CalculateFromErfassungsPeriodList(erfassungsPeriod, erfassungsPeriodList);

                switch (erfassungsPeriod.NetzErfassungsmodus)
                {
                    case NetzErfassungsmodus.Summarisch:
                        continue;
                    case NetzErfassungsmodus.Tabellarisch:

                        var zustandsabschnittList = filtererFactory
                            .CreateFilterer<Zustandsabschnitt>(parameter)
                            .Filter(transactionScopeProvider.Queryable<Zustandsabschnitt>().Where(za => za.Strassenabschnitt.ErfassungsPeriod == erfassungsPeriod))
                            .Fetch(s => s.Strassenabschnitt)
                            .ToArray();
                        CalculatePosForErfassungsPeriod<Zustandsabschnitt, Strassenabschnitt>(zustandsabschnittList, jahresInterval, erfassungsPeriod);
                        break;
                    case NetzErfassungsmodus.Gis:
                        var zustandsabschnittGisList = filtererFactory
                            .CreateFilterer<ZustandsabschnittGIS>(parameter)
                            .Filter(transactionScopeProvider.Queryable<ZustandsabschnittGIS>().Where(za => za.StrassenabschnittGIS.ErfassungsPeriod == erfassungsPeriod))
                            .Fetch(s => s.StrassenabschnittGIS)
                            .ToArray();
                        CalculatePosForErfassungsPeriod<ZustandsabschnittGIS, StrassenabschnittGIS>(zustandsabschnittGisList, jahresInterval, erfassungsPeriod);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("NetzErfassungsmodus");
                }
            }
        }

        private void CalculatePosForErfassungsPeriod<TZustandsabschnittBase, TStrassenabschnittBase>(TZustandsabschnittBase[] zustandsabschnittList, JahresInterval jahresInterval, ErfassungsPeriod erfassungsPeriod) 
            where TStrassenabschnittBase : StrassenabschnittBase
            where TZustandsabschnittBase : ZustandsabschnittBase
        {
            var zustandsspiegelProJahrGrafischeDiagramPos = Enum.GetValues(typeof (ZustandsindexTyp))
                .OfType<ZustandsindexTyp>()
                .Where(z => z != ZustandsindexTyp.Unbekannt)
                .Select(z => CreateDiagramPo(z, jahresInterval))
                .ToDictionary(po => po.ZustandsindexTyp, po => po);

            decimal totalUnknowZustandsindexTypFlaeche = 0;

            decimal mittlereZustandsindexWithFlaeche = 0;
            decimal mittlererAufnahmedatumTickMultipliedWithFlaeche = 0;
            
            foreach (var zustandsabschnitt in zustandsabschnittList)
            {
                var zustandsindexTyp = ZustandsindexCalculator.GetTyp(zustandsabschnitt.Zustandsindex);
                zustandsspiegelProJahrGrafischeDiagramPos[zustandsindexTyp].FlaecheFahrbahn += (zustandsabschnitt.FlaecheFahrbahn ?? 0);
                mittlereZustandsindexWithFlaeche += zustandsabschnitt.Zustandsindex * (zustandsabschnitt.FlaecheFahrbahn ?? 0);

                mittlererAufnahmedatumTickMultipliedWithFlaeche += zustandsabschnitt.Aufnahmedatum.Ticks * (zustandsabschnitt.FlaecheFahrbahn ?? 0);
            }

            var currentJahr = ErfassungsPeriodService.GetCurrentErfassungsPeriod().Erfassungsjahr.Year;

            diagramPos[jahresInterval.JahrBis] = zustandsspiegelProJahrGrafischeDiagramPos.Values.ToList();

            var totalKnownZustandsindexTypFlaeche = zustandsspiegelProJahrGrafischeDiagramPos.Values.Sum(po => po.FlaecheFahrbahn);
            var totalFlaeche = totalKnownZustandsindexTypFlaeche + totalUnknowZustandsindexTypFlaeche;
            
            var totalStrasseFlaeche = transactionScopeProvider.Queryable<TStrassenabschnittBase>()
                .Where(z => z.ErfassungsPeriod == erfassungsPeriod)
                .Sum(g => g.Laenge * g.BreiteFahrbahn);

            var zustandsspiegelProJahrGrafischeTablePos = zustandsspiegelProJahrGrafischeDiagramPos.Values
                .Select(po => CreateTablePo(jahresInterval, GetPerzent(po.FlaecheFahrbahn, totalKnownZustandsindexTypFlaeche), FormatHelper.ToReportNoDecimalPercentString, LocalizationService.GetLocalizedEnum(po.ZustandsindexTyp), (int)po.ZustandsindexTyp, currentJahr, po.ZustandsindexTyp));
            tablePos[jahresInterval.JahrBis] = zustandsspiegelProJahrGrafischeTablePos.ToList();

            PercentPartitioningCorrector.Corrigate(tablePos[jahresInterval.JahrBis].Cast<IPercentHolder>().ToList());

            var mittlererAufnahmedatum = (mittlererAufnahmedatumTickMultipliedWithFlaeche == 0 || totalFlaeche == 0) ? (decimal?)null : (decimal.Divide(mittlererAufnahmedatumTickMultipliedWithFlaeche, totalFlaeche));
            tablePos[jahresInterval.JahrBis].Add(CreateTablePo(jahresInterval, mittlererAufnahmedatum, d => FormatHelper.ToReportDateTimeString(d, "-"), reportLocalizationService.MittleresAlterDerZustandsaufnahmen, -10, currentJahr));
            
            var netzAnteil = GetPerzent(totalKnownZustandsindexTypFlaeche, totalStrasseFlaeche);
            tablePos[jahresInterval.JahrBis].Add(CreateTablePo(jahresInterval, netzAnteil, FormatHelper.ToReportNoDecimalPercentString, reportLocalizationService.NetzAnteil, -20, currentJahr));
            
            var mittlererZustandsindex = (mittlereZustandsindexWithFlaeche == 0 || totalFlaeche == 0) ? 0 : decimal.Divide(mittlereZustandsindexWithFlaeche, totalFlaeche);
            tablePos[jahresInterval.JahrBis].Add(CreateTablePo(jahresInterval, mittlererZustandsindex, FormatHelper.ToReportDecimalString, reportLocalizationService.MittlererZustandsindex, -30, currentJahr));
        }

        private static decimal GetPerzent(decimal totalKnownZustandsindexTypFlaeche, decimal totalFlaeche)
        {
            if (totalFlaeche > 0)
                return totalKnownZustandsindexTypFlaeche / totalFlaeche * 100;
            return 0;
        }

        private TablePo CreateTablePo(JahresInterval jahresInterval, decimal? decimalValue, Func<decimal?, string> format, string zustandsindexTypBezeichnung, int sortOrder,  int currentYear, ZustandsindexTyp zustandsindexTyp = ZustandsindexTyp.Unbekannt)
        {
            return new TablePo
                {
                    CurrentJahr = currentYear,
                    AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                    JahrVon = jahresInterval.JahrVon,
                    JahrBis = jahresInterval.JahrBis,
                    DecimalValue = decimalValue,
                    Format = format,
                    ZustandsindexTyp = zustandsindexTyp,
                    ZustandsindexTypBezeichnung = zustandsindexTypBezeichnung,
                    SortOrder = sortOrder,
                    LegendImageUrl = reportLegendImageService.GetLegendUrlForEnum(zustandsindexTyp)
                };
        }

        private DiagramPo CreateDiagramPo(ZustandsindexTyp z, JahresInterval jahresInterval)
        {
            return new DiagramPo
                       {
                           ZustandsindexTyp = z,
                           ZustandsindexTypBezeichnung = LocalizationService.GetLocalizedEnum(z),
                           ColorCode = z.ToColorCode(),
                           JahrVon = jahresInterval.JahrVon,
                           JahrBis = jahresInterval.JahrBis
                       };
        }
        
        protected override void CustomizeSubReport(XmlDocument doc, XmlNamespaceManager nsmgr, string subReportKey)
        {
            base.CustomizeSubReport(doc, nsmgr, subReportKey);

            if (subReportKey == FilterListSubReportName)
                return;

            var diagrammColumnCount = GetDiagrammColumnCount(subReportKey);

            ReportSizeCollection reportSizeCollection = reportResourceLocator.GetReportSizeCollection<ZustandsspiegelProJahrGrafischePoProvider>();

            var reportSize = reportSizeCollection.ReportSizes.Single(r => r.ColumnCount == diagrammColumnCount);

            decimal leftMargin = 2.0m;
            decimal rightMargin = 0.0m;

            var currentChartWidth = ReportTemplatingService.GetChartWidth(doc, nsmgr);
            var chartInnerPlotPostionLeftPercent = ReportTemplatingService.GetChartInnerPlotPostionLeftPercent(doc, nsmgr);

            var currentChartPlotAreaWidth = currentChartWidth - (currentChartWidth / 100.0m * chartInnerPlotPostionLeftPercent);

            var columnCountDependentAdditionalWidth = (ReportTemplatingService.GetMatrixTableColumnWidth(doc, nsmgr) + reportSize.ColumnWidthCorrection)*(diagrammColumnCount - 1);
            var chartWidth = currentChartPlotAreaWidth + columnCountDependentAdditionalWidth;

            var totalWidth = chartWidth + leftMargin + rightMargin;

            ReportTemplatingService.SetChartInnerPlotPostionLeft(doc, nsmgr, leftMargin / totalWidth * 100);
            ReportTemplatingService.SetChartInnerPlotPostionWidth(doc, nsmgr, chartWidth / totalWidth * 100);
            
            ReportTemplatingService.SetChartLeft(doc, nsmgr, reportSize.ChartLeft);
            ReportTemplatingService.SetChartWidth(doc, nsmgr, totalWidth);
        }

        private int GetDiagrammColumnCount(string subReportKey)
        {
            int diagrammColumnCount;
            if (subReportKey == FullSubReportName)
                diagrammColumnCount = MaxColumnCount;
            else
            {
                diagrammColumnCount = tablePos.Count % MaxColumnCount;
                if (diagrammColumnCount == 0)
                    diagrammColumnCount = MaxColumnCount;
            }
            return diagrammColumnCount;
        }

        protected override void OnSubReportProcessing(object sender, ServerSubReportProcessingEventArgs args)
        {
            base.OnSubReportProcessing(sender, args);

            if (args.ReportPath == PartialSubReportName || args.ReportPath == FullSubReportName)
            {
                var page = int.Parse(args.Parameters["Page"].Values[0]);

                var pagedDiagramPos = diagramPos.OrderBy(d => d.Key).Skip(page * MaxColumnCount).Take(MaxColumnCount).SelectMany(i => i.Value).ToList();
                var pagedTablePos = tablePos.OrderBy(d => d.Key).Skip(page * MaxColumnCount).Take(MaxColumnCount).SelectMany(i => i.Value).ToList();
                ReportDataSource.SetDataSources(args.DataSources, new List<ReportDataSource>
                {
                    new ReportDataSource(pagedDiagramPos, ReportDataSourceFactory),
                    new ReportDataSource(pagedTablePos, ReportDataSourceFactory)
                });
            }
        }

        protected override void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames)
        {
            base.AddSubReportDefinitionResourceNames(subReportDefinitionResourceNames);
            subReportDefinitionResourceNames.Add(FullSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(PartialSubReportName, SubReportDefinitionResourceName);
        }

        protected override void BuildFilterList(IFilterListBuilder<Parameter> builder)
        {
            base.BuildFilterList(builder);

            builder.AddFilterListItem(p => p.ErfassungsPeriodIdVon, p => LocalizeErfassungsPeriod(p.ErfassungsPeriodIdVon));
            builder.AddFilterListItem(p => p.ErfassungsPeriodIdBis, p => LocalizeErfassungsPeriod(p.ErfassungsPeriodIdBis));
            builder.AddFilterListItem(p => p.Eigentuemer);
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }

        public override int? MaxImagePreviewPageHeight { get { return 530; } }
    }
}