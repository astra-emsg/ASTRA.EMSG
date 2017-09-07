using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;
using Parameter = ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische.WiederbeschaffungswertUndWertverlustProJahrGrafischeParameter;
using DiagramPo = ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische.WiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPo;
using TablePo = ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische.WiederbeschaffungswertUndWertverlustProJahrGrafischeTablePo;
using MainPo = ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische.WiederbeschaffungswertUndWertverlustProJahrGrafischeMainPo;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische
{
    public interface IWiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W5_3)]
    public class WiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider : EmsgFilterablePoProviderBase<Parameter, WiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPo>, IWiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider
    {
        private readonly IJahresIntervalService jahresIntervalService;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;
        private readonly IReportLegendImageService reportLegendImageService;
        private const int MaxColumnCount = 9;

        private const string FullSubReportName = "WiederbeschaffungswertUndWertverlustProJahrGrafischeFullSubReport";
        private const string PartialSubReportName = "WiederbeschaffungswertUndWertverlustProJahrGrafischeePartialSubReport";

        public WiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider(
            IJahresIntervalService jahresIntervalService,
            IFiltererFactory filtererFactory,
            ITransactionScopeProvider transactionScopeProvider,
            IReportLocalizationService reportLocalizationService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService,
            INetzSummarischDetailService netzSummarischDetailService,
            IBelastungskategorieService belastungskategorieService,
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService,
            IReportLegendImageService reportLegendImageService)
        {
            this.jahresIntervalService = jahresIntervalService;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.reportLocalizationService = reportLocalizationService;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
            this.netzSummarischDetailService = netzSummarischDetailService;
            this.belastungskategorieService = belastungskategorieService;
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.reportLegendImageService = reportLegendImageService;
            diagramPos = new Dictionary<int, List<DiagramPo>>();
            tablePos = new Dictionary<int, List<TablePo>>();
        }

        protected override bool IsForClosedErfassungsPeriod(Parameter parameter)
        {
            var erfassungsPeriod = ErfassungsPeriodService.GetEntityById(parameter.JahrIdBis);
            return erfassungsPeriod != null && erfassungsPeriod.IsClosed;
        }

        private readonly Dictionary<int, List<DiagramPo>> diagramPos;
        private readonly Dictionary<int, List<TablePo>> tablePos;

        public override void LoadDataSources(Parameter parameter)
        {
            base.LoadDataSources(parameter);
            CalculatePos(parameter);
            HasData = tablePos.Values.SelectMany(o => o).Any(p => p.Value > 0);
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
            int jahrVon = GetJahr(parameter.JahrIdVon);
            int jahrBis = GetJahr(parameter.JahrIdBis);

            var kenngroessenFruehererJahreList = kenngroessenFruehererJahreService.GetKenngroessenFruehererJahren(jahrVon, jahrBis);
            var erfassungsPeriodList = ErfassungsPeriodService.GetErfassungsPeriods(jahrVon, jahrBis);

            //If there is a Filter for not supported Field, Skip year
            if (parameter.Eigentuemer == null)
            {
                for (int i = 0; i < kenngroessenFruehererJahreList.Count; i++)
                {
                    var kenngroessenFruehererJahre = kenngroessenFruehererJahreList[i];

                    int erfassungsJahrVon = kenngroessenFruehererJahre.Jahr;
                    int erfassungsJahrBis;

                    if (kenngroessenFruehererJahre == kenngroessenFruehererJahreList.Last())
                        erfassungsJahrBis = erfassungsPeriodList.Any() ? erfassungsPeriodList.First().Erfassungsjahr.Year - 1 : erfassungsJahrVon;
                    else
                        erfassungsJahrBis = kenngroessenFruehererJahreList[i + 1].Jahr - 1;

                    CalculatePosForKenngroessenFruehererJahre(ErfassungsPeriodService.GetCurrentErfassungsPeriod(), kenngroessenFruehererJahre, new JahresInterval(erfassungsJahrVon, erfassungsJahrBis));
                }
            }

            for (int i = 0; i < erfassungsPeriodList.Count; i++)
            {
                var erfassungsPeriod = erfassungsPeriodList[i];
                var jahresInterval = jahresIntervalService.CalculateFromErfassungsPeriodList(erfassungsPeriod, erfassungsPeriodList);

                switch (erfassungsPeriod.NetzErfassungsmodus)
                {
                    case NetzErfassungsmodus.Summarisch:
                        //If there is a Filter for not supported Field, Skip year
                        if (parameter.Eigentuemer != null)
                            continue;

                        var netzSummarischDetailList = netzSummarischDetailService.GetEntitiesBy(erfassungsPeriod).Fetch(nsd => nsd.Belastungskategorie).ToList();
                        CalculatePosForSummarischeModus(erfassungsPeriod, netzSummarischDetailList, jahresInterval);
                        break;
                    case NetzErfassungsmodus.Tabellarisch:
                        var strassenabschnittList = transactionScopeProvider.Queryable<Strassenabschnitt>().Where(sa => sa.ErfassungsPeriod == erfassungsPeriod);
                        CalculatePosForStrassenModus(erfassungsPeriod, strassenabschnittList, parameter, jahresInterval);
                        break;
                    case NetzErfassungsmodus.Gis:
                        var strassenabschnittGisList = transactionScopeProvider.Queryable<StrassenabschnittGIS>().Where(sa => sa.ErfassungsPeriod == erfassungsPeriod);
                        CalculatePosForStrassenModus(erfassungsPeriod, strassenabschnittGisList, parameter, jahresInterval);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("NetzErfassungsmodus");
                }
            }
        }

        private void CalculatePosForStrassenModus<T>(ErfassungsPeriod erfassungsPeriod, IQueryable<T> queryable, Parameter parameter, JahresInterval jahresInterval)
            where T : StrassenabschnittBase
        {
            var strassenabschnittList = filtererFactory
               .CreateFilterer<T>(parameter)
               .Filter(queryable)
               .Fetch(sa => sa.Belastungskategorie)
               .ToList();

            CalculatePosForJahr(erfassungsPeriod, strassenabschnittList, jahresInterval, GetWiederbeschaffungswert, sa => sa.Belastungskategorie, sa => sa.FlaecheFahrbahn);
        }

        private void CalculatePosForSummarischeModus(ErfassungsPeriod erfassungsPeriod, List<NetzSummarischDetail> netzSummarischDetailList, JahresInterval jahresInterval)
        {
            CalculatePosForJahr(erfassungsPeriod, netzSummarischDetailList, jahresInterval, GetWiederbeschaffungswert, nsd => nsd.Belastungskategorie, nsd => nsd.Fahrbahnflaeche);
        }

        private void CalculatePosForKenngroessenFruehererJahre(ErfassungsPeriod erfassungsPeriod, KenngroessenFruehererJahre kenngroessenFruehererJahre, JahresInterval jahresInterval)
        {
            CalculatePosForJahr(erfassungsPeriod, kenngroessenFruehererJahre.KenngroesseFruehereJahrDetails.ToList(), jahresInterval, GetWiederbeschaffungswert, kgfjd => kgfjd.Belastungskategorie, kfjd => kfjd.Fahrbahnflaeche);
        }

        private void CalculatePosForJahr<T>(ErfassungsPeriod erfassungsPeriod, List<T> entityList, JahresInterval jahresInterval,
                                            Func<T, WiederbeschaffungswertKatalogModel, decimal> getWiederBeschaffungswert,
                                            Func<T, Belastungskategorie> getBelastungskategorie,
                                            Func<T, decimal> getFlaeche)
        {

            if (!diagramPos.ContainsKey(jahresInterval.JahrBis))
                diagramPos[jahresInterval.JahrBis] = new List<DiagramPo>();
            if (!tablePos.ContainsKey(jahresInterval.JahrBis))
                tablePos[jahresInterval.JahrBis] = new List<TablePo>();

            var wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos = GetWiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos(jahresInterval);

            foreach (var netzSummarischDetail in entityList)
            {
                var belastungskategorie = getBelastungskategorie(netzSummarischDetail);
                var wieder = wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(belastungskategorie, erfassungsPeriod);
                var wiederbeschaffungswert = getWiederBeschaffungswert(netzSummarischDetail, wieder);

                var diagramPo = wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos[belastungskategorie.Id];
                //Note: divided by 1 000 (1000m2)
                diagramPo.FlaecheFahrbahn += getFlaeche(netzSummarischDetail) / 1000m;
                //Note: divided by 1 000 000 (Mio. CHF) infro from sample excel
                diagramPo.WiederBeschaffungsWert += wiederbeschaffungswert / 1000000m;
                //Note: divided by 1 000 (kCHF)
                diagramPo.WertVerlust += (wiederbeschaffungswert * wieder.AlterungsbeiwertII / 100) / 1000;
            }

            diagramPos[jahresInterval.JahrBis].AddRange(wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos.Values);
            var currentJahr = ErfassungsPeriodService.GetCurrentErfassungsPeriod().Erfassungsjahr.Year;
            var wiederbeschaffungswertUndWertverlustProJahrGrafischeTablePos
                = wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos.Values
                    .Select(po => new TablePo
                                      {
                                          CurrentJahr = currentJahr,
                                          AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                          JahrVon = jahresInterval.JahrVon,
                                          JahrBis = jahresInterval.JahrBis,
                                          Value = po.FlaecheFahrbahn,
                                          Bezeichnung = po.BelastungskategorieBezeichnung + " " + reportLocalizationService.TausendQuadratMeter,
                                          ColorCode = po.ColorCode,
                                          LegendUrl = reportLegendImageService.GetLegendUrl(po.BelastungskategorieTyp),
                                          SortOrder = belastungskategorieService.AlleBelastungskategorie.IndexOf(belastungskategorieService.AlleBelastungskategorie.Single(bk => bk.Id == po.BelastungskategorieId))
                                      });

            tablePos[jahresInterval.JahrBis].AddRange(wiederbeschaffungswertUndWertverlustProJahrGrafischeTablePos);

            tablePos[jahresInterval.JahrBis].Add(new TablePo
                             {
                                 CurrentJahr = currentJahr,
                                 AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                 JahrVon = jahresInterval.JahrVon,
                                 JahrBis = jahresInterval.JahrBis,
                                 Value = wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos.Values.Sum(po => po.FlaecheFahrbahn),
                                 Bezeichnung = reportLocalizationService.GesamtFlaeche,
                                 SortOrder = 10
                             });

            tablePos[jahresInterval.JahrBis].Add(new TablePo
                             {
                                 CurrentJahr = currentJahr,
                                 AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                 JahrVon = jahresInterval.JahrVon,
                                 JahrBis = jahresInterval.JahrBis,
                                 Value = wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos.Values.Sum(po => po.WertVerlust),
                                 Bezeichnung = reportLocalizationService.WV,
                                 ColorCode = "#8b0000",
                                 LegendUrl = reportLegendImageService.GetLegendUrl("WV"),
                                 SortOrder = 20
                             });

            tablePos[jahresInterval.JahrBis].Add(new TablePo
                             {
                                 CurrentJahr = currentJahr,
                                 AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                 JahrVon = jahresInterval.JahrVon,
                                 JahrBis = jahresInterval.JahrBis,
                                 Value = wiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos.Values.Sum(po => po.WiederBeschaffungsWert),
                                 Bezeichnung = reportLocalizationService.WBW,
                                 LegendUrl = reportLegendImageService.GetLegendUrl("WBW"),
                                 ColorCode = "#cd48ff",
                                 SortOrder = 30
                             });
        }

        protected override void CustomizeSubReport(XmlDocument doc, XmlNamespaceManager nsmgr, string subReportKey)
        {
            base.CustomizeSubReport(doc, nsmgr, subReportKey);

            if (subReportKey == FilterListSubReportName)
                return;

            var diagrammColumnCount = GetDiagrammColumnCount(subReportKey);
            ReportTemplatingService.AdjustChartSizeForColumnCount<WiederbeschaffungswertUndWertverlustProJahrGrafischePoProvider>(doc, nsmgr, diagrammColumnCount);
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

        public override int? MaxImagePreviewPageHeight { get { return 712; } }

        protected override void AddSubReportDefinitionResourceNames(Dictionary<string, string> subReportDefinitionResourceNames)
        {
            base.AddSubReportDefinitionResourceNames(subReportDefinitionResourceNames);
            var value = "ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProJahrGrafische.WiederbeschaffungswertUndWertverlustProJahrGrafischeSubReport.rdlc";
            subReportDefinitionResourceNames.Add(FullSubReportName, value);
            subReportDefinitionResourceNames.Add(PartialSubReportName, value);
        }

        protected override void SetReportParameters(Parameter parameter)
        {
            base.SetReportParameters(parameter);
            AddReportParameter("LeftAxisLabel", reportLocalizationService.FlaecheAxis);
            AddReportParameter("RightAxisLabel", reportLocalizationService.WBWAxis);
            AddReportParameter("LeftAxisMaximum", (tablePos.Values.SelectMany(v => v).Where(b => b.Bezeichnung == reportLocalizationService.GesamtFlaeche).Max(c => (decimal?)c.Value) ?? 0) * 1.1m);
            AddReportParameter("RightAxisMaximum", (tablePos.Values.SelectMany(v => v).Where(b => b.Bezeichnung == reportLocalizationService.WBW || b.Bezeichnung == reportLocalizationService.WV).Max(c => (decimal?)c.Value) ?? 0) * 1.1m);
        }

        private decimal GetWiederbeschaffungswert(StrassenabschnittBase strassenabschnittBase, WiederbeschaffungswertKatalogModel wieder)
        {
            //ToDo: Clarify! Should we calculate with Trottoir?
            if (strassenabschnittBase.HasTrottoirInformation)
                return strassenabschnittBase.FlaecheFahrbahn * wieder.FlaecheFahrbahn + wieder.FlaecheTrottoir * strassenabschnittBase.FlaecheTrottoir;

            return strassenabschnittBase.GesamtFlaeche * wieder.GesamtflaecheFahrbahn;
        }

        private decimal GetWiederbeschaffungswert(NetzSummarischDetail netzSummarischDetail, WiederbeschaffungswertKatalogModel wieder)
        {
            return netzSummarischDetail.Fahrbahnflaeche * wieder.GesamtflaecheFahrbahn;
        }

        private decimal GetWiederbeschaffungswert(KenngroessenFruehererJahreDetail kenngroessenFruehererJahreDetail, WiederbeschaffungswertKatalogModel wieder)
        {
            return kenngroessenFruehererJahreDetail.Fahrbahnflaeche * wieder.GesamtflaecheFahrbahn;
        }

        private string GetLocalizedJahr(Guid jahrId)
        {
            var kenngroessenFruehererJahreVon = kenngroessenFruehererJahreService.GetEntityById(jahrId);
            if (kenngroessenFruehererJahreVon != null)
                return kenngroessenFruehererJahreVon.Jahr.ToString();

            return LocalizeErfassungsPeriod(jahrId);
        }

        private int GetJahr(Guid jahrId)
        {
            var kenngroessenFruehererJahreVon = kenngroessenFruehererJahreService.GetEntityById(jahrId);
            if (kenngroessenFruehererJahreVon != null)
                return kenngroessenFruehererJahreVon.Jahr;

            return ErfassungsPeriodService.GetEntityById(jahrId).Erfassungsjahr.Year;;
        }

        private Dictionary<Guid, DiagramPo> GetWiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPos(JahresInterval jahresInterval)
        {
            return belastungskategorieService.AlleBelastungskategorie
                .Select(bk => CreateWiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPo(bk, jahresInterval))
                .ToDictionary(po => po.BelastungskategorieId, po => po);
        }

        private DiagramPo CreateWiederbeschaffungswertUndWertverlustProJahrGrafischeDiagramPo(Belastungskategorie belastungskategorie, JahresInterval jahresInterval)
        {
            return new DiagramPo
                       {
                           ColorCode = belastungskategorie.ColorCode,
                           JahrVon = jahresInterval.JahrVon,
                           JahrBis = jahresInterval.JahrBis,
                           BelastungskategorieId = belastungskategorie.Id,
                           BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(belastungskategorie.Typ),
                           BelastungskategorieReihenfolge = belastungskategorie.Reihenfolge,
                           BelastungskategorieTyp = belastungskategorie.Typ
                       };
        }

        protected override void BuildFilterList(IFilterListBuilder<Parameter> builder)
        {
            base.BuildFilterList(builder);
            builder.AddFilterListItem(p => p.JahrIdVon, p => GetLocalizedJahr(p.JahrIdVon));
            builder.AddFilterListItem(p => p.JahrIdBis, p => GetLocalizedJahr(p.JahrIdBis));
            builder.AddFilterListItem(p => p.Eigentuemer);
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }
    }
}
