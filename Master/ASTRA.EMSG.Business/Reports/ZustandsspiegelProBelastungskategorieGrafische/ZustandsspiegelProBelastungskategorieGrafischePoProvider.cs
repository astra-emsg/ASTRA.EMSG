using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Utils;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Master.Calculators;
using NHibernate.Linq;
using Parameter = ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische.ZustandsspiegelProBelastungskategorieGrafischeParameter;
using DiagramPo = ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische.ZustandsspiegelProBelastungskategorieGrafischeDiagramPo;
using TablePo = ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische.ZustandsspiegelProBelastungskategorieGrafischeTablePo;
using MainPo = ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische.ZustandsspiegelProBelastungskategorieGrafischePo;


namespace ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische
{
    public interface IZustandsspiegelProBelastungskategorieGrafischePoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_2, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    [ReportInfo(AuswertungTyp.W3_2, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    public class ZustandsspiegelProBelastungskategorieGrafischePoProvider : EmsgModeDependentPoProviderBase<Parameter, MainPo>, IZustandsspiegelProBelastungskategorieGrafischePoProvider
    {
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IFiltererFactory filtererFactory;
        private readonly IReportLegendImageService reportLegendImageService;
        private readonly IReportResourceLocator reportResourceLocator;

        private const string FullSubReportName = "ZustandsspiegelProBelastungskategorieGrafischeFullSubReport";
        private const string PartialSubReportName = "ZustandsspiegelProBelastungskategorieGrafischePartialSubReport";

        private const string SubReportDefinitionResourceName = "ASTRA.EMSG.Business.Reports.ZustandsspiegelProBelastungskategorieGrafische.ZustandsspiegelProBelastungskategorieGrafischeSubReport.rdlc";

        private const int MaxColumnCount = 12;

        private readonly Dictionary<Guid, Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, DiagramPo>>> diagramPos = new Dictionary<Guid, Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, DiagramPo>>>();
        private readonly Dictionary<Guid, Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, List<TablePo>>>> tablePos = new Dictionary<Guid, Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, List<TablePo>>>>();

        private readonly Dictionary<Guid, Dictionary<FlaecheTyp, decimal>> mittleresAlterForBelastungskategorien = new Dictionary<Guid, Dictionary<FlaecheTyp, decimal>>();
        private readonly Dictionary<Guid, Dictionary<FlaecheTyp, decimal>> strassenabschnittFlaecheProBelastungskategorie = new Dictionary<Guid, Dictionary<FlaecheTyp, decimal>>();

        private readonly Dictionary<Guid, Dictionary<FlaecheTyp, decimal>> totalZustandsabschnittFlaechenUnbekanntDictionary = new Dictionary<Guid, Dictionary<FlaecheTyp, decimal>>();
        private readonly Dictionary<Guid, Dictionary<FlaecheTyp, decimal>> totalZustandsabschnittFlaechenDictionary = new Dictionary<Guid, Dictionary<FlaecheTyp, decimal>>();
        
        private readonly List<ZustandsindexTyp> zustandsindexTypList;
        private readonly List<FlaecheTyp> flaecheTypList;

        protected int TotalColumnCount { get { return belastungskategorieService.AlleBelastungskategorie.Count * 2; } }

        public ZustandsspiegelProBelastungskategorieGrafischePoProvider(
            IBelastungskategorieService belastungskategorieService,
            IReportLocalizationService reportLocalizationService,
            IFiltererFactory filtererFactory,
            ITransactionScopeProvider transactionScopeProvider,
            IReportLegendImageService reportLegendImageService,
            IReportResourceLocator reportResourceLocator)
        {
            this.belastungskategorieService = belastungskategorieService;
            this.reportLocalizationService = reportLocalizationService;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.reportLegendImageService = reportLegendImageService;
            this.reportResourceLocator = reportResourceLocator;

            zustandsindexTypList = Enum.GetValues(typeof(ZustandsindexTyp)).OfType<ZustandsindexTyp>().ToList();
            flaecheTypList = new List<FlaecheTyp>{FlaecheTyp.Fahrbahn, FlaecheTyp.Trottoir};
        }

        protected override List<MainPo> GetPresentationObjectListForSummarisch(Parameter parameter)
        {
            return NotSupported();
        }
        
        protected override List<MainPo> GetPresentationObjectListForTabellarisch(Parameter parameter)
        {
            var zustandsabschnittList = FilterEntities<Zustandsabschnitt>(parameter).Fetch(z => z.Strassenabschnitt).ThenFetch(s => s.Belastungskategorie).ToList();
            var strassenabschnittList = FilterEntities<Strassenabschnitt>(parameter).Fetch(s => s.Belastungskategorie).ToList();

            CalculatePos(zustandsabschnittList, strassenabschnittList);

            return GetZustandsspiegelProBelastungskategorieGrafischePos();
        }

        protected override List<MainPo> GetPresentationObjectListForGis(Parameter parameter)
        {
            var zustandsabschnittGisList = FilterEntities<ZustandsabschnittGIS>(parameter).Fetch(z => z.StrassenabschnittGIS).ThenFetch(s => s.Belastungskategorie).ToList();
            var strassenabschnittGisList = FilterEntities<StrassenabschnittGIS>(parameter).Fetch(s => s.Belastungskategorie).ToList();

            CalculatePos(zustandsabschnittGisList, strassenabschnittGisList);

            return GetZustandsspiegelProBelastungskategorieGrafischePos();
        }

        private IQueryable<TEntity> FilterEntities<TEntity>(Parameter parameter)
            where TEntity : class, IEntity
        {
            return filtererFactory.CreateFilterer<TEntity>(parameter).Filter(transactionScopeProvider.Queryable<TEntity>());
        }
        
        private List<MainPo> GetZustandsspiegelProBelastungskategorieGrafischePos()
        {
            return Enumerable.Range(0, (TotalColumnCount / MaxColumnCount)).Select(i => new MainPo { Page = i }).ToList();
        }

        private void CalculatePos<TZustandsabschnitt, TStrassenabschnitt>(List<TZustandsabschnitt> zustandsabschnitten, List<TStrassenabschnitt> strassenabschnitten)
            where TZustandsabschnitt : ZustandsabschnittBase
            where TStrassenabschnitt : StrassenabschnittBase
        {
            CalculateStrassenabschnittFlaechenProBelastungskategorie(strassenabschnitten);

            CalculateDiagramPos();

            CalculateMittleresAlterForBelastungskategorien(zustandsabschnitten);

            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                CalculateTotalZustandsabschnittFlaechen(belastungskategorie);

                CalculateTablePosForZustandsindexTypen(belastungskategorie);
                CalculateTablePosForAdditionalRows(belastungskategorie);
            }
        }

        private void CalculateStrassenabschnittFlaechenProBelastungskategorie<TStrassenabschnitt>(List<TStrassenabschnitt> strassenabschnittBaseList)
            where TStrassenabschnitt : StrassenabschnittBase
        {
            var strassenabschnittenBelastungskategorienGroup = strassenabschnittBaseList.GroupBy(s => new {s.Belastungskategorie}).ToArray();

            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                var belastungskategorieGroup = strassenabschnittenBelastungskategorienGroup.SingleOrDefault(g => g.Key.Belastungskategorie.Id == belastungskategorie.Id);

                if (!strassenabschnittFlaecheProBelastungskategorie.ContainsKey(belastungskategorie.Id))
                    strassenabschnittFlaecheProBelastungskategorie[belastungskategorie.Id] = new Dictionary<FlaecheTyp, decimal> {{FlaecheTyp.Fahrbahn, 0}, {FlaecheTyp.Trottoir, 0}};

                strassenabschnittFlaecheProBelastungskategorie[belastungskategorie.Id][FlaecheTyp.Fahrbahn] = belastungskategorieGroup != null ? belastungskategorieGroup.Sum(g => g.FlaecheFahrbahn) : 0;
                strassenabschnittFlaecheProBelastungskategorie[belastungskategorie.Id][FlaecheTyp.Trottoir] = belastungskategorieGroup != null ? belastungskategorieGroup.Sum(g => g.FlaecheTrottoir) : 0;
            }
        }

        private void CalculateDiagramPos()
        {
            int sortOrder = 0;
            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                diagramPos[belastungskategorie.Id] = new Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, DiagramPo>>();
                foreach (var flaecheTyp in flaecheTypList)
                {
                    diagramPos[belastungskategorie.Id][flaecheTyp] = new Dictionary<ZustandsindexTyp, DiagramPo>();
                    foreach (var zustandsindexTyp in zustandsindexTypList)
                    {
                        var bezeichnung = string.Format("{0}-{1}", LocalizationService.GetLocalizedBelastungskategorieTyp(belastungskategorie.Typ, LocalizationType.Short), LocalizationService.GetLocalizedEnum(flaecheTyp));
                        diagramPos[belastungskategorie.Id][flaecheTyp][zustandsindexTyp] = CreateDiagramPo(zustandsindexTyp, sortOrder, flaecheTyp, bezeichnung);
                    }
                    sortOrder++;
                }
            }
        }

        private void CalculateMittleresAlterForBelastungskategorien<TZustandsabschnitt>(List<TZustandsabschnitt> zustandsabschnittBaseList)
            where TZustandsabschnitt : ZustandsabschnittBase
        {
            foreach (var zustandsabschnitt in zustandsabschnittBaseList)
            {
                var belastungskategorieId = zustandsabschnitt.StrassenabschnittBase.Belastungskategorie.Id;
                var belastungskategorieDictionary = diagramPos[belastungskategorieId];

                belastungskategorieDictionary[FlaecheTyp.Fahrbahn][ZustandsindexCalculator.GetTyp(zustandsabschnitt.Zustandsindex)].Value += zustandsabschnitt.FlaecheFahrbahn ?? 0;
                belastungskategorieDictionary[FlaecheTyp.Trottoir][zustandsabschnitt.ZustandsindexTrottoirLinks].Value += zustandsabschnitt.FlaceheTrottoirLinks ?? 0;
                belastungskategorieDictionary[FlaecheTyp.Trottoir][zustandsabschnitt.ZustandsindexTrottoirRechts].Value += zustandsabschnitt.FlaceheTrottoirRechts ?? 0;

                if (!mittleresAlterForBelastungskategorien.ContainsKey(belastungskategorieId))
                    mittleresAlterForBelastungskategorien[belastungskategorieId] = new Dictionary<FlaecheTyp, decimal>{{FlaecheTyp.Fahrbahn, 0}, {FlaecheTyp.Trottoir, 0}};

                mittleresAlterForBelastungskategorien[belastungskategorieId][FlaecheTyp.Fahrbahn] += (zustandsabschnitt.FlaecheFahrbahn ?? 0)*zustandsabschnitt.Aufnahmedatum.Ticks;
                mittleresAlterForBelastungskategorien[belastungskategorieId][FlaecheTyp.Trottoir] += (zustandsabschnitt.FlaecheTrottoir)*zustandsabschnitt.Aufnahmedatum.Ticks;
            }
        }

        private void CalculateTablePosForZustandsindexTypen(Belastungskategorie belastungskategorie)
        {
            tablePos[belastungskategorie.Id] = new Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, List<TablePo>>>();
            foreach (var flaecheTyp in flaecheTypList)
            {
                tablePos[belastungskategorie.Id][flaecheTyp] = new Dictionary<ZustandsindexTyp, List<TablePo>>();
                foreach (var zustandsindexTyp in zustandsindexTypList.Where(zt => zt != ZustandsindexTyp.Unbekannt))
                {
                    var diagramPo = diagramPos[belastungskategorie.Id][flaecheTyp][zustandsindexTyp];

                    var totalFlaeche = totalZustandsabschnittFlaechenDictionary[belastungskategorie.Id][flaecheTyp];
                    var percentValue = totalFlaeche > 0
                                          ? (diagramPo.Value/totalFlaeche*100)
                                          : (decimal?) null;

                    tablePos[belastungskategorie.Id][flaecheTyp][zustandsindexTyp] =
                        new List<TablePo>
                            {
                                CreateTablePo(percentValue, FormatHelper.ToReportNoDecimalPercentString, diagramPo, LocalizationService.GetLocalizedEnum(diagramPo.ZustandsindexTyp), diagramPo.RowSortOrder)
                            };
                }
            }

            PercentPartitioningCorrector.Corrigate(tablePos.SelectMany(ddl => ddl.Value).SelectMany(dl => dl.Value).SelectMany(l => l.Value).Cast<IPercentHolder>().ToList());
        }

        private void CalculateTablePosForAdditionalRows(Belastungskategorie belastungskategorie)
        {
            int minZustandsIndexSortOrder = zustandsindexTypList.Select(zt => (int)zt).Min();
            
            foreach (var flaecheTyp in flaecheTypList)
            {
                tablePos[belastungskategorie.Id][flaecheTyp][ZustandsindexTyp.Unbekannt] = new List<TablePo>();

                var diagramPo = diagramPos[belastungskategorie.Id][flaecheTyp][ZustandsindexTyp.Unbekannt];

                var totalZustandsabschnittFlaeche = totalZustandsabschnittFlaechenDictionary[belastungskategorie.Id][flaecheTyp];
                var totalZustandsabschnittFlaecheUnbekannt = totalZustandsabschnittFlaechenUnbekanntDictionary[belastungskategorie.Id][flaecheTyp];

                //Netz Anteil summary row
                var netzAnteilValue = totalZustandsabschnittFlaeche + totalZustandsabschnittFlaecheUnbekannt > 0
                                          ? ((totalZustandsabschnittFlaeche / strassenabschnittFlaecheProBelastungskategorie[belastungskategorie.Id][flaecheTyp]) * 100)
                                          : (decimal?) null;

                var netzAnteil = CreateTablePo(netzAnteilValue, FormatHelper.ToReportNoDecimalPercentString, diagramPo, reportLocalizationService.NetzAnteil, minZustandsIndexSortOrder - 1);
                tablePos[belastungskategorie.Id][flaecheTyp][ZustandsindexTyp.Unbekannt].Add(netzAnteil);

                //Mittleres Alter summary Row
                decimal? mittleresAlterValue = totalZustandsabschnittFlaeche + totalZustandsabschnittFlaecheUnbekannt > 0
                                              ? (mittleresAlterForBelastungskategorien[belastungskategorie.Id][flaecheTyp]/(totalZustandsabschnittFlaeche + totalZustandsabschnittFlaecheUnbekannt))
                                              : (decimal?)null;

                var mittleresAlter = CreateTablePo(mittleresAlterValue, FormatHelper.ToReportDateTimeString, diagramPo, reportLocalizationService.MittleresAlterDerZustandsaufnahmen, minZustandsIndexSortOrder - 2);
                tablePos[belastungskategorie.Id][flaecheTyp][ZustandsindexTyp.Unbekannt].Add(mittleresAlter);
            }
        }

        private void CalculateTotalZustandsabschnittFlaechen(Belastungskategorie belastungskategorie)
        {
            Dictionary<FlaecheTyp, Dictionary<ZustandsindexTyp, DiagramPo>> diagramPoDictionaryForBelastungskategorie = diagramPos[belastungskategorie.Id];

            totalZustandsabschnittFlaechenDictionary[belastungskategorie.Id] = flaecheTypList
                .ToDictionary(ft => ft, ft => diagramPoDictionaryForBelastungskategorie[ft].Values
                                                  .Where(po => po.ZustandsindexTyp != ZustandsindexTyp.Unbekannt)
                                                  .Sum(po => po.Value));

            totalZustandsabschnittFlaechenUnbekanntDictionary[belastungskategorie.Id] = flaecheTypList
                .ToDictionary(ft => ft, ft => diagramPoDictionaryForBelastungskategorie[ft].Values
                                                  .Where(po => po.ZustandsindexTyp == ZustandsindexTyp.Unbekannt)
                                                  .Sum(po => po.Value));
        }
        
        private DiagramPo CreateDiagramPo(ZustandsindexTyp zustandsindexTyp, int sortOrder, FlaecheTyp flaecheTyp, string bezeichnung)
        {
            return new DiagramPo
                {
                    Value = 0,
                    ZustandsindexTyp = zustandsindexTyp,
                    ColumnSortOrder = sortOrder,
                    RowSortOrder = (int) zustandsindexTyp,
                    ColorCode = zustandsindexTyp.ToColorCode(),
                    FlaecheTyp = flaecheTyp,
                    Bezeichnung = bezeichnung
                };
        }

        private TablePo CreateTablePo(decimal? decimalValue, Func<decimal?, string> format, DiagramPo diagramPo, string rowBezeichnung, int rowSortOrder)
        {
            return new TablePo
                {
                    DecimalValue = decimalValue,
                    Format = format,
                    RowSortOrder = rowSortOrder,
                    ColumnSortOrder = diagramPo.ColumnSortOrder,
                    ColumnBezeichnung = diagramPo.Bezeichnung,
                    RowBezeichnung = rowBezeichnung,
                    LegendImageUrl = reportLegendImageService.GetLegendUrlForEnum(diagramPo.ZustandsindexTyp),
                    ColorCode = diagramPo.ColorCode,
                    IsSummaryRow = diagramPo.ZustandsindexTyp == ZustandsindexTyp.Unbekannt
                };
        }

        protected override void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames)
        {
            base.AddSubReportDefinitionResourceNames(subReportDefinitionResourceNames);
            subReportDefinitionResourceNames.Add(FullSubReportName, SubReportDefinitionResourceName);
            subReportDefinitionResourceNames.Add(PartialSubReportName, SubReportDefinitionResourceName);
        }

        protected override void CustomizeSubReport(XmlDocument doc, XmlNamespaceManager nsmgr, string subReportKey)
        {
            base.CustomizeSubReport(doc, nsmgr, subReportKey);

            if (subReportKey == FilterListSubReportName)
                return;

            var diagrammColumnCount = GetDiagrammColumnCount(subReportKey);

            ReportSizeCollection reportSizeCollection = reportResourceLocator.GetReportSizeCollection<ZustandsspiegelProBelastungskategorieGrafischePoProvider>();

            var reportSize = reportSizeCollection.ReportSizes.Single(r => r.ColumnCount == diagrammColumnCount);

            decimal leftMargin = 2.0m;
            decimal rightMargin = 0.0m;

            var chartWidth = ((ReportTemplatingService.GetMatrixTableColumnWidth(doc, nsmgr) + reportSize.ColumnWidthCorrection) * (diagrammColumnCount + 1));
            
            var totalWidth = chartWidth + leftMargin + rightMargin;

            ReportTemplatingService.SetChartInnerPlotPostionLeft(doc, nsmgr, leftMargin/totalWidth*100);
            ReportTemplatingService.SetChartInnerPlotPostionWidth(doc, nsmgr, chartWidth/totalWidth*100);

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
                diagrammColumnCount = TotalColumnCount % MaxColumnCount;
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

                var pagedDiagramPos = diagramPos.Values
                    .SelectMany(dictionary => dictionary.Values)
                    .OrderBy(po => po.Values.First().ColumnSortOrder)
                    .Skip(page * MaxColumnCount).Take(MaxColumnCount)
                    .SelectMany(dictionary => dictionary.Values)
                    .ToList();

                var pagedTablePos = tablePos.Values.SelectMany(dictionary => dictionary.Values)
                    .OrderBy(po => po.Values.First().First().ColumnSortOrder)
                    .Skip(page * MaxColumnCount).Take(MaxColumnCount)
                    .SelectMany(dictionary => dictionary.Values)
                    .SelectMany(list => list)
                    .ToList();

                ReportDataSource.SetDataSources(args.DataSources, new List<ReportDataSource>
                    {
                        new ReportDataSource(pagedDiagramPos, ReportDataSourceFactory),
                        new ReportDataSource(pagedTablePos, ReportDataSourceFactory)
                    });
            }
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }

        public override int? MaxImagePreviewPageHeight { get { return 520; } }

        protected override void BuildFilterList(IFilterListBuilder<Parameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
        }
    }
}
