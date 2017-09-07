using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.MengeProBelastungskategorieGrafische
{
    public interface IMengeProBelastungskategorieGrafischePoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W1_1)]
    public class MengeProBelastungskategorieGrafischePoProvider : EmsgModeDependentPoProviderBase<MengeProBelastungskategorieGrafischeParameter, MengeProBelastungskategorieGrafischePo>, IMengeProBelastungskategorieGrafischePoProvider
    {
        private readonly INetzSummarischService netzSummarischService;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;

        public MengeProBelastungskategorieGrafischePoProvider(INetzSummarischService netzSummarischService,
            IReportLocalizationService reportLocalizationService, IBelastungskategorieService belastungskategorieService,
            IStrassenabschnittService strassenabschnittService, IStrassenabschnittGISService strassenabschnittGISService)
        {
            this.netzSummarischService = netzSummarischService;
            this.reportLocalizationService = reportLocalizationService;
            this.belastungskategorieService = belastungskategorieService;
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGISService = strassenabschnittGISService;
        }

        protected override List<MengeProBelastungskategorieGrafischePo> GetPresentationObjectListForSummarisch(MengeProBelastungskategorieGrafischeParameter parameter)
        {
            return netzSummarischService.GetNetzSummarisch(GetErfassungsPeriod(parameter.ErfassungsPeriodId))
                .NetzSummarischDetails
                .Select(CreateMengeProBelastungskategorieGrafischePo)
                .ToList();
        }

        private MengeProBelastungskategorieGrafischePo CreateMengeProBelastungskategorieGrafischePo(NetzSummarischDetail ns)
        {
            return new MengeProBelastungskategorieGrafischePo
                       {
                           BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(ns.BelastungskategorieTyp),
                           BelastungskategorieTyp = ns.BelastungskategorieTyp,
                           ColorCode = ns.Belastungskategorie.ColorCode,
                           Menge = ns.Fahrbahnflaeche,
                           BelastungskategorieReihenfolge = ns.Belastungskategorie.Reihenfolge,
                           MengeArtKurzBezeichnung = reportLocalizationService.Fahrbahnflaeche
                       };
        }

        protected override List<MengeProBelastungskategorieGrafischePo> GetPresentationObjectListForTabellarisch(MengeProBelastungskategorieGrafischeParameter parameter)
        {
            var strassenabschnittQuery = strassenabschnittService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId));
            return GetMengeProBelastungskategorieGrafischePos(parameter, strassenabschnittQuery);
        }

        protected override List<MengeProBelastungskategorieGrafischePo> GetPresentationObjectListForGis(MengeProBelastungskategorieGrafischeParameter parameter)
        {
            var strassenabschnittGisQuery = strassenabschnittGISService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId));
            return GetMengeProBelastungskategorieGrafischePos(parameter, strassenabschnittGisQuery);
        }

        private List<MengeProBelastungskategorieGrafischePo> GetMengeProBelastungskategorieGrafischePos(MengeProBelastungskategorieGrafischeParameter parameter, IQueryable<StrassenabschnittBase> strassenabschnittGisQuery)
        {
            if (parameter.Eigentuemer.HasValue)
                strassenabschnittGisQuery = strassenabschnittGisQuery.Where(sa => sa.Strasseneigentuemer == parameter.Eigentuemer).Fetch(sa => sa.Belastungskategorie);

            var strassenabschnittBaseList = strassenabschnittGisQuery.Fetch(s => s.Belastungskategorie).ToList();

            if (!strassenabschnittBaseList.Any())
                return new List<MengeProBelastungskategorieGrafischePo>();

            return GetMengeProBelastungskategorieGrafischePos(strassenabschnittBaseList);
        }

        private List<MengeProBelastungskategorieGrafischePo> GetMengeProBelastungskategorieGrafischePos(IEnumerable<StrassenabschnittBase> strassenabschnittBaseList)
        {
            List<MengeProBelastungskategorieGrafischePo> mengeProBelastungskategoriePos = strassenabschnittBaseList
                .GroupBy(s => s.Belastungskategorie)
                .SelectMany(g => new List<MengeProBelastungskategorieGrafischePo>
                {
                    new MengeProBelastungskategorieGrafischePo
                        {
                            BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(g.Key.Typ),
                            BelastungskategorieTyp = g.Key.Typ,
                            BelastungskategorieReihenfolge = g.Key.Reihenfolge,
                            ColorCode = g.Key.ColorCode,
                            Menge = Math.Round(g.Select(s => s.Laenge * s.BreiteFahrbahn).Sum(s => s)),
                            MengeArtKurzBezeichnung = reportLocalizationService.Fahrbahnflaeche
                        },
                    new MengeProBelastungskategorieGrafischePo
                        {
                            BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(g.Key.Typ),
                            BelastungskategorieTyp = g.Key.Typ,
                            BelastungskategorieReihenfolge = g.Key.Reihenfolge,
                            ColorCode = g.Key.ColorCode,
                            Menge = Math.Round(g.Select(s => s.Laenge * ((s.BreiteTrottoirLinks ?? 0) + (s.BreiteTrottoirRechts ?? 0))).Sum(s => s)),
                            MengeArtKurzBezeichnung = reportLocalizationService.Trottoirflaeche
                        }
                }).ToList();

            foreach (var bk in belastungskategorieService.AllBelastungskategorieModel)
            {
                AddEmptyPoIfNeeded(mengeProBelastungskategoriePos, bk, reportLocalizationService.Fahrbahnflaeche);
                AddEmptyPoIfNeeded(mengeProBelastungskategoriePos, bk, reportLocalizationService.Trottoirflaeche);
            }

            return mengeProBelastungskategoriePos.OrderBy(po => po.BelastungskategorieTyp).ToList();
        }

        private void AddEmptyPoIfNeeded(List<MengeProBelastungskategorieGrafischePo> mengeProBelastungskategoriePos, BelastungskategorieModel bk, string mengeArtKurzBezeichnung)
        {
            if (!mengeProBelastungskategoriePos.Any(po => po.BelastungskategorieTyp == bk.Typ && po.MengeArtKurzBezeichnung == mengeArtKurzBezeichnung))
            {
                mengeProBelastungskategoriePos.Add(new MengeProBelastungskategorieGrafischePo
                                                       {
                                                           BelastungskategorieTyp = bk.Typ,
                                                           BelastungskategorieKurzBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(bk.Typ),
                                                           BelastungskategorieReihenfolge = bk.Reihenfolge,
                                                           MengeArtKurzBezeichnung = mengeArtKurzBezeichnung,
                                                           ColorCode = bk.ColorCode
                                                       });
            }
        }

        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }

        protected override void BuildFilterList(IFilterListBuilder<MengeProBelastungskategorieGrafischeParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
        }

        public override int? MaxImagePreviewPageHeight { get { return 540; } }
    }
}