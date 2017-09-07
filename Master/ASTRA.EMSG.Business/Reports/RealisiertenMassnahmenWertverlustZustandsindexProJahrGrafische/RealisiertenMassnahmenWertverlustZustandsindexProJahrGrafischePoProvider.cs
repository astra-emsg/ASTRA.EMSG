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
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

using Parameter = ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter;
using DiagramPo = ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo;
using TablePo = ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeTablePo;
using MainPo = ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeMainPo;

namespace ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische
{
    public class RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeMainPo
    {
        public int Page { get; set; }
    }

    public interface IRealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W5_4)]
    public class RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider :
        EmsgFilterablePoProviderBase
            <RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter,
            RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo>,
        IRealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider
    {
        private readonly IJahresIntervalService jahresIntervalService;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider transactionScopeProvider;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IKenngroessenFruehererJahreService kenngroessenFruehererJahreService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;
        private readonly IReportLegendImageService reportLegendImageService;
        private const int MaxColumnCount = 12;

        private const string FullSubReportName = "RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeFullSubReport";
        private const string PartialSubReportName = "RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePartialSubReport";

        public RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider(
            IJahresIntervalService jahresIntervalService,
            IFiltererFactory filtererFactory,
            ITransactionScopeProvider transactionScopeProvider,
            IReportLocalizationService reportLocalizationService,
            IKenngroessenFruehererJahreService kenngroessenFruehererJahreService,
            INetzSummarischDetailService netzSummarischDetailService,
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService,
            IReportLegendImageService reportLegendImageService)
        {
            this.jahresIntervalService = jahresIntervalService;
            this.filtererFactory = filtererFactory;
            this.transactionScopeProvider = transactionScopeProvider;
            this.reportLocalizationService = reportLocalizationService;
            this.kenngroessenFruehererJahreService = kenngroessenFruehererJahreService;
            this.netzSummarischDetailService = netzSummarischDetailService;
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.reportLegendImageService = reportLegendImageService;

            diagramPos = new Dictionary<int, List<DiagramPo>>();
            tablePos = new Dictionary<int, List<TablePo>>();
        }

        private readonly Dictionary<int, List<DiagramPo>> diagramPos;
        private readonly Dictionary<int, List<TablePo>> tablePos;

        protected override bool IsForClosedErfassungsPeriod(
            RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter)
        {
            var erfassungsPeriod = ErfassungsPeriodService.GetEntityById(parameter.JahrIdBis);
            return erfassungsPeriod != null && erfassungsPeriod.IsClosed;
        }

        public override void LoadDataSources(
            RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter)
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
                var mainPos = Enumerable.Range(0, (tablePos.Count / MaxColumnCount)).Select(i => new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeMainPo() { Page = i }).ToList();
                return new List<ReportDataSource> 
                { 
                    new ReportDataSource(mainPos, ReportDataSourceFactory),
                };
            }
        }

        private void CalculatePos(RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter)
        {
            int jahrVon = GetJahr(parameter.JahrIdVon);
            int jahrBis = GetJahr(parameter.JahrIdBis);

            var kenngroessenFruehererJahreList =
                kenngroessenFruehererJahreService.GetKenngroessenFruehererJahren(jahrVon, jahrBis);
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
                        erfassungsJahrBis = erfassungsPeriodList.Any()
                                                ? erfassungsPeriodList.First().Erfassungsjahr.Year - 1
                                                : erfassungsJahrVon;
                    else
                        erfassungsJahrBis = kenngroessenFruehererJahreList[i + 1].Jahr - 1;
                    CalculatePosForKenngroessenFruehererJahre(ErfassungsPeriodService.GetCurrentErfassungsPeriod(),
                                                              kenngroessenFruehererJahre, new JahresInterval(erfassungsJahrVon, erfassungsJahrBis));
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
                        CalculatePosForSummarischeModus(erfassungsPeriod, jahresInterval);
                        break;
                    case NetzErfassungsmodus.Tabellarisch:
                        CalculatePosForStrassenModus(erfassungsPeriod, parameter, jahresInterval);
                        break;
                    case NetzErfassungsmodus.Gis:
                        CalculatePosForStrassenModusGIS(erfassungsPeriod, parameter, jahresInterval);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("NetzErfassungsmodus");
                }
            }
        }

        private void CalculatePosForStrassenModus(ErfassungsPeriod erfassungsPeriod, RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter, JahresInterval jahresInterval)
        {
            var strassenabschnittList = filtererFactory
               .CreateFilterer<Strassenabschnitt>(parameter)
               .Filter(transactionScopeProvider.Queryable<Strassenabschnitt>()
               .Where(e => e.ErfassungsPeriod == erfassungsPeriod))
               .Fetch(sa => sa.Belastungskategorie)
               .FetchMany(sa => sa.Zustandsabschnitten)
               .ToList();

            var diagramPo = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo()
            {
                JahrVon = jahresInterval.JahrVon,
                JahrBis = jahresInterval.JahrBis,
                WertVerlust = strassenabschnittList.Sum(d => (GetWiederbeschaffungswert(d, wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod)) * wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod).AlterungsbeiwertII / 100) / 1000m),
                MittlererZustandindex = SafeDevide(strassenabschnittList.SelectMany(s => s.Zustandsabschnitten).Sum(d => (d.FlaecheFahrbahn ?? 0) * d.Zustandsindex), strassenabschnittList.SelectMany(s => s.Zustandsabschnitten).Sum(z => z.FlaecheFahrbahn) ?? 0m),
                RealisierteMassnahmen = (transactionScopeProvider.Queryable<RealisierteMassnahme>().Where(r => r.ErfassungsPeriod == erfassungsPeriod).ToArray().Sum(s => (s.KostenFahrbahn ?? 0) + (s.KostenTrottoirLinks ?? 0) + (s.KostenTrottoirRechts ?? 0))) / 1000m
            };

            EnsureJahrInPoLists(jahresInterval);
            diagramPos[jahresInterval.JahrBis].Add(diagramPo);
            CalculateTablePos(jahresInterval, diagramPo);
        }

        public class StrasenabschnittGISReportPO : IFlaecheProvider
        {
            public Guid Id { get; set; }
            public TrottoirTyp Trottoir { get; set; }
            public decimal Laenge { get; set; }
            public decimal BreiteFahrbahn { get; set; }
            public decimal? BreiteTrottoirLinks { get; set; }
            public decimal? BreiteTrottoirRechts { get; set; }
            public Belastungskategorie Belastungskategorie { get; set; }
        }

        private void CalculatePosForStrassenModusGIS(ErfassungsPeriod erfassungsPeriod, RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter, JahresInterval jahresInterval)
        {
            var strassenabschnittList = filtererFactory
               .CreateFilterer<StrassenabschnittGIS>(parameter)
               .Filter(transactionScopeProvider.Queryable<StrassenabschnittGIS>().Fetch(s => s.Belastungskategorie)
               .Where(e => e.ErfassungsPeriod == erfassungsPeriod))
                .Select(s =>
                    new StrasenabschnittGISReportPO
                        {
                            Id = s.Id,
                            Trottoir = s.Trottoir,
                            Laenge = s.Laenge,
                            BreiteFahrbahn = s.BreiteFahrbahn,
                            BreiteTrottoirLinks = s.BreiteTrottoirLinks,
                            BreiteTrottoirRechts = s.BreiteTrottoirRechts,
                            Belastungskategorie = s.Belastungskategorie
                        })
               .ToList();
            IEnumerable<Guid> strabsIds = strassenabschnittList.Select(i => i.Id).ToArray();
            var zustandabscnitten = transactionScopeProvider.Queryable<ZustandsabschnittGIS>()
                                                            .Where(z => strabsIds.Contains(z.StrassenabschnittGIS.Id))
                                                            .Select(s => new
                                                                {
                                                                    s.Laenge,
                                                                    s.StrassenabschnittGIS.BreiteFahrbahn,
                                                                    s.Zustandsindex
                                                                })
                                                            .ToArray();

            var diagramPo = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo()
            {
                JahrVon = jahresInterval.JahrVon,
                JahrBis = jahresInterval.JahrBis,
                WertVerlust = strassenabschnittList.Sum(d => (GetWiederbeschaffungswert(d, wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod)) * wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod).AlterungsbeiwertII / 100) / 1000m),
                MittlererZustandindex = SafeDevide(zustandabscnitten.Sum(d => (d.Laenge * d.BreiteFahrbahn) * d.Zustandsindex), zustandabscnitten.Sum(z => z.Laenge * z.BreiteFahrbahn)),
                RealisierteMassnahmen = (transactionScopeProvider.Queryable<RealisierteMassnahmeGIS>().Where(r => r.ErfassungsPeriod == erfassungsPeriod).ToArray().Sum(s => (s.KostenFahrbahn ?? 0) + (s.KostenTrottoirLinks ?? 0) + (s.KostenTrottoirRechts ?? 0))) / 1000m
            };

            EnsureJahrInPoLists(jahresInterval);
            diagramPos[jahresInterval.JahrBis].Add(diagramPo);
            CalculateTablePos(jahresInterval, diagramPo);
        }

        private void EnsureJahrInPoLists(JahresInterval jahresInterval)
        {
            if (!diagramPos.ContainsKey(jahresInterval.JahrBis))
                diagramPos[jahresInterval.JahrBis] = new List<DiagramPo>();
            if (!tablePos.ContainsKey(jahresInterval.JahrBis))
                tablePos[jahresInterval.JahrBis] = new List<TablePo>();
        }

        private decimal SafeDevide(decimal dividend, decimal divisor)
        {
            return dividend / (divisor == 0 ? 1 : divisor);
        }

        private void CalculatePosForSummarischeModus(ErfassungsPeriod erfassungsPeriod, JahresInterval jahresInterval)
        {
            var netzSummarischDetailList = netzSummarischDetailService.GetEntitiesBy(erfassungsPeriod).Fetch(nsd => nsd.Belastungskategorie).ToList();

            var diagramPo = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo()
            {
                JahrVon = jahresInterval.JahrVon,
                JahrBis = jahresInterval.JahrBis,
                WertVerlust = netzSummarischDetailList.Sum(d => (GetWiederbeschaffungswert(d, wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod)) * wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod).AlterungsbeiwertII / 100) / 1000m),
                MittlererZustandindex = netzSummarischDetailList.Average(d => d.MittlererZustand),
                RealisierteMassnahmen = (transactionScopeProvider.Queryable<RealisierteMassnahmeSummarsich>().Where(r => r.ErfassungsPeriod == erfassungsPeriod).Sum(s => s.KostenFahrbahn) ?? 0) / 1000m
            };

            EnsureJahrInPoLists(jahresInterval);
            diagramPos[jahresInterval.JahrBis].Add(diagramPo);
            CalculateTablePos(jahresInterval, diagramPo);
        }

        private void CalculatePosForKenngroessenFruehererJahre(ErfassungsPeriod erfassungsPeriod,
                                                               KenngroessenFruehererJahre kenngroessenFruehererJahre,
                                                               JahresInterval jahresInterval)
        {
            var diagramPo = new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo()
            {
                JahrVon = jahresInterval.JahrVon,
                JahrBis = jahresInterval.JahrBis,
                WertVerlust = kenngroessenFruehererJahre.KenngroesseFruehereJahrDetails.Sum(d => (GetWiederbeschaffungswert(d, wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod)) * wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(d.Belastungskategorie, erfassungsPeriod).AlterungsbeiwertII / 100) / 1000m),
                MittlererZustandindex = kenngroessenFruehererJahre.KenngroesseFruehereJahrDetails.Average(d => d.MittlererZustand) ?? 0,
                RealisierteMassnahmen = kenngroessenFruehererJahre.KostenFuerWerterhaltung / 1000m
            };

            EnsureJahrInPoLists(jahresInterval);
            diagramPos[jahresInterval.JahrBis].Add(diagramPo);
            CalculateTablePos(jahresInterval, diagramPo);
        }

        private void CalculateTablePos(JahresInterval jahresInterval,
                                       RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo diagramPo)
        {
            var currentJahr = ErfassungsPeriodService.GetCurrentErfassungsPeriod().Erfassungsjahr.Year;

            tablePos[jahresInterval.JahrBis].Add(new TablePo
                             {
                                 CurrentJahr = currentJahr,
                                 AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                 JahrVon = jahresInterval.JahrVon,
                                 JahrBis = jahresInterval.JahrBis,
                                 Value = diagramPo.RealisierteMassnahmen,
                                 Bezeichnung = reportLocalizationService.RealisiertenMassnahmen,
                                 FormatString = FormatStrings.ReportNoDecimalFormat,
                                 ColorCode = "#800080",
                                 LegendUrl = reportLegendImageService.GetLegendUrl("RM"),
                                 SortOrder = 10
                             });


            tablePos[jahresInterval.JahrBis].Add(new TablePo
                             {
                                 CurrentJahr = currentJahr,
                                 AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                 JahrVon = jahresInterval.JahrVon,
                                 JahrBis = jahresInterval.JahrBis,
                                 Value = diagramPo.WertVerlust,
                                 Bezeichnung = reportLocalizationService.WV,
                                 FormatString = FormatStrings.ReportNoDecimalFormat,
                                 ColorCode = "#6495ED",
                                 LegendUrl = reportLegendImageService.GetLegendUrl("RWV"),
                                 SortOrder = 10
                             });

            tablePos[jahresInterval.JahrBis].Add(new TablePo
                             {
                                 CurrentJahr = currentJahr,
                                 AktualString = LocalizationService.GetLocalizedText("CurrentShort"),
                                 JahrVon = jahresInterval.JahrVon,
                                 JahrBis = jahresInterval.JahrBis,
                                 Value = diagramPo.MittlererZustandindex,
                                 Bezeichnung = reportLocalizationService.MittlererZustandsindex,
                                 ColorCode = "#FF0000",
                                 FormatString = FormatStrings.ReportShortDecimalFormat,
                                 LegendUrl = reportLegendImageService.GetLegendUrl("Mittlerer"),
                                 SortOrder = 20
                             });
        }

        protected override void CustomizeSubReport(XmlDocument doc, XmlNamespaceManager nsmgr, string subReportKey)
        {
            base.CustomizeSubReport(doc, nsmgr, subReportKey);

            if (subReportKey == FilterListSubReportName)
                return;

            var diagrammColumnCount = GetDiagrammColumnCount(subReportKey);
            ReportTemplatingService.AdjustChartSizeForColumnCount<RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischePoProvider>(doc, nsmgr, diagrammColumnCount);
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
            var value = "ASTRA.EMSG.Business.Reports.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafische.RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeSubReport.rdlc";
            subReportDefinitionResourceNames.Add(FullSubReportName, value);
            subReportDefinitionResourceNames.Add(PartialSubReportName, value);
        }

        protected override void SetReportParameters(RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeParameter parameter)
        {
            base.SetReportParameters(parameter);
            AddReportParameter("LeftAxisLabel", reportLocalizationService.Kosten);
            AddReportParameter("RightAxisLabel", reportLocalizationService.MittlererZustandsindex);
            AddReportParameter("LeftAxisMaximum", (tablePos.Values.SelectMany(v => v).Where(b => b.Bezeichnung == reportLocalizationService.RealisiertenMassnahmen || b.Bezeichnung == reportLocalizationService.WV).Max(c => (decimal?)c.Value) ?? 0) * 1.1m);
            AddReportParameter("RightAxisMaximum", 0);
        }

        private decimal GetWiederbeschaffungswert(IFlaecheProvider strassenabschnittBase, WiederbeschaffungswertKatalogModel wieder)
        {
            //ToDo: Clarify! Should we calculate with Trottoir?
            if (strassenabschnittBase.HasTrottoirInformation())
                return strassenabschnittBase.FlaecheFahrbahn() * wieder.FlaecheFahrbahn + wieder.FlaecheTrottoir * strassenabschnittBase.FlaecheTrottoir();

            return strassenabschnittBase.GesamtFlaeche() * wieder.GesamtflaecheFahrbahn;
        }

        private decimal GetWiederbeschaffungswert(NetzSummarischDetail netzSummarischDetail, WiederbeschaffungswertKatalogModel wieder)
        {
            return netzSummarischDetail.Fahrbahnflaeche * wieder.GesamtflaecheFahrbahn;
        }

        private decimal GetWiederbeschaffungswert(KenngroessenFruehererJahreDetail kenngroessenFruehererJahreDetail, WiederbeschaffungswertKatalogModel wieder)
        {
            return kenngroessenFruehererJahreDetail.Fahrbahnflaeche * wieder.GesamtflaecheFahrbahn;
        }

        private int GetJahr(Guid jahrId)
        {
            var kenngroessenFruehererJahreVon = kenngroessenFruehererJahreService.GetEntityById(jahrId);
            if (kenngroessenFruehererJahreVon != null)
                return kenngroessenFruehererJahreVon.Jahr;

            return ErfassungsPeriodService.GetEntityById(jahrId).Erfassungsjahr.Year;
        }

        private string GetLocalizedJahr(Guid jahrId)
        {
            var kenngroessenFruehererJahreVon = kenngroessenFruehererJahreService.GetEntityById(jahrId);
            if (kenngroessenFruehererJahreVon != null)
                return kenngroessenFruehererJahreVon.Jahr.ToString();

            return LocalizeErfassungsPeriod(jahrId);
        }

        private RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo CreateRealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo(Belastungskategorie belastungskategorie, int erfassungsJahrVon, int erfassungsJahrBis)
        {
            return new RealisiertenMassnahmenWertverlustZustandsindexProJahrGrafischeDiagramPo
                       {
                           ColorCode = belastungskategorie.ColorCode,
                           JahrVon = erfassungsJahrVon,
                           JahrBis = erfassungsJahrBis,
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

        public override int? MaxImagePreviewPageHeight { get { return 500; } }
    }
}
