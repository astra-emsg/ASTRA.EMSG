using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.ReflectionMappingConfiguration;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Services.SchadenMetadaten;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden
{
    public interface IAusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider
    {
        List<Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden> GetPreviewModels(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter, IPresentationObjectProcessor<Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden> presentationObjectProcessor);
    }

    [ReportInfo(AuswertungTyp.W3_3, NetzErfassungsmodus = NetzErfassungsmodus.Tabellarisch)]
    [ReportInfo(AuswertungTyp.W3_3, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class AusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider : EmsgModeDependentPoProviderBase<AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter, AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo>, IAusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider
    {
        private readonly IZustandsabschnittService zustandsabschnittService;
        private readonly IZustandsabschnittGISService zustandsabschnittGISService;
        private readonly IEntityServiceMappingEngine entityServiceMappingEngine;
        private readonly IFiltererFactory filtererFactory;
        private readonly ITransactionScopeProvider scopeProvider;
        private readonly ILocalizationService localizationService;
        private readonly ISchadenMetadatenService schadenMetadatenService;

        public AusgefuellteErfassungsformulareFuerOberflaechenschaedenPoProvider(
            IZustandsabschnittService zustandsabschnittService,
            ILocalizationService localizationService,
            ISchadenMetadatenService schadenMetadatenService,
            IZustandsabschnittGISService zustandsabschnittGISService,
            IEntityServiceMappingEngine entityServiceMappingEngine,
            IFiltererFactory filtererFactory,
            ITransactionScopeProvider scopeProvider
            )
        {
            this.zustandsabschnittService = zustandsabschnittService;
            this.localizationService = localizationService;
            this.schadenMetadatenService = schadenMetadatenService;
            this.zustandsabschnittGISService = zustandsabschnittGISService;
            this.entityServiceMappingEngine = entityServiceMappingEngine;
            this.filtererFactory = filtererFactory;
            this.scopeProvider = scopeProvider;
        }

        public List<Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden> GetPreviewModels(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter, IPresentationObjectProcessor<Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden> presentationObjectProcessor)
        {
            if (AvailableInCurrentErfassungPeriod || parameter.ErfassungsPeriodId != null)
            {
                var erfassungsmodus = GetNetzErfassungsmodus(parameter);

                switch (erfassungsmodus)
                {
                    case NetzErfassungsmodus.Summarisch:
                        return new List<Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden>();
                    case NetzErfassungsmodus.Tabellarisch:
                        return GetZustandsabschnitten(parameter).Fetch(za => za.Strassenabschnitt)
                            .Select(entityServiceMappingEngine.Translate<Zustandsabschnitt, Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden>).ToList();
                    case NetzErfassungsmodus.Gis:
                        var erfassungPeriod = GetErfassungsPeriod(parameter);
                        //NOTE: Manually Fetch because a random OracleClient memory access exception 
                        var routeGis = scopeProvider.Queryable<InspektionsRouteGIS>().Where(e => e.ErfassungsPeriod == erfassungPeriod).ToList();
                        return GetZustandsabschnittenGIS(parameter)
                            .Fetch(za => za.StrassenabschnittGIS)
                            .ThenFetchMany(sa => sa.InspektionsRtStrAbschnitte)//.ThenFetch(irsa => irsa.InspektionsRouteGIS)
                            .Select(entityServiceMappingEngine.Translate<ZustandsabschnittGIS, Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden>).ToList();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new List<Models.Reports.AusgefuellteErfassungsformulareFuerOberflaechenschaeden>();
        }

        protected override List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPresentationObjectListForSummarisch(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            return new List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo>();
        }

        protected override List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPresentationObjectListForTabellarisch(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            return GetZustandsabschnitten(parameter)
                .Where(za => za.Erfassungsmodus == ZustandsErfassungsmodus.Detail)
                .Fetch(za => za.Strassenabschnitt)
                .FetchMany(za => za.Schadendetails)
                .ToList()
                .SelectMany(GetPos)
                .ToList();
        }

        protected override List<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPresentationObjectListForGis(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            return GetZustandsabschnittenGIS(parameter)
                .Where(za => za.Erfassungsmodus == ZustandsErfassungsmodus.Detail)
                .Fetch(za => za.StrassenabschnittGIS)
                .FetchMany(za => za.Schadendetails)
                .ToList()
                .SelectMany(GetPos)
                .ToList();
        }

        private IQueryable<ZustandsabschnittGIS> GetZustandsabschnittenGIS(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            var zustandsabschnitten = zustandsabschnittGISService.GetEntitiesBy(GetErfassungsPeriod(parameter));

            zustandsabschnitten = filtererFactory.CreateFilterer<ZustandsabschnittGIS>(parameter).Filter(zustandsabschnitten);

            return zustandsabschnitten;
        }

        private IQueryable<Zustandsabschnitt> GetZustandsabschnitten(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            var zustandsabschnitten = zustandsabschnittService.GetEntitiesBy(GetErfassungsPeriod(parameter));

            zustandsabschnitten = filtererFactory.CreateFilterer<Zustandsabschnitt>(parameter).Filter(zustandsabschnitten);

            return zustandsabschnitten;
        }

        private IEnumerable<AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo> GetPos(ZustandsabschnittBase za)
        {
            var pos = schadenMetadatenService
                .GetSchadengruppeMetadaten(za.StrassenabschnittBase.Belag)
                .SelectMany(sgm => sgm.Schadendetails.Select(sdm => CreatePo(za, sgm, sdm)))
                .OrderBy(po => po.SchadengruppeReihung)
                .ThenBy(po => po.SchadendetailReihung)
                .ToList();

            var group = pos.GroupBy(po => po.SchadengruppeTyp, (key, g) => g.Max(po => po.Matrix) * (g.Any() ? g.First().Gewicht : 0));
            decimal? schadensumme = group.Sum();

            foreach (var po in pos)
            {
                po.Schadensumme = schadensumme;
                po.Zustandsindex = schadensumme.HasValue ? Math.Min(schadensumme.Value * 0.1m, 5.0m) : (decimal?)null;
            }

            return pos;
        }

        private AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo CreatePo(ZustandsabschnittBase za, SchadengruppeMetadaten sgm, SchadendetailMetadaten sdm)
        {
            Schadendetail schadendetail = za.Schadendetails.SingleOrDefault(sd => sd.SchadendetailTyp == sdm.SchadendetailTyp);

            return new AusgefuellteErfassungsformulareFuerOberflaechenschaedenPo
                       {
                           StrassenabschnittId = za.StrassenabschnittBase.Id,
                           ZustandsabschnittId = za.Id,

                           Strassenname = za.StrassenabschnittBase.Strassenname,
                           ZustandsabschnittBezeichnungVon = za.BezeichnungVon,
                           ZustandsabschnittBezeichnungBis = za.BezeichnungBis,

                           Laenge = za.Laenge,
                           FlaecheFahrbahn = za.StrassenabschnittBase.FlaecheFahrbahn,
                           AufnahmeDatum = za.Aufnahmedatum,
                           Aufnahmeteam = za.Aufnahmeteam,
                           Wetter = za.Wetter,
                           WetterBezeichnung = LocalizationService.GetLocalizedEnum(za.Wetter),
                           Bemerkung = za.Bemerkung,

                           SchadengruppeTyp = sgm.SchadengruppeTyp,
                           SchadengruppeBezeichnung = localizationService.GetLocalizedEnum(sgm.SchadengruppeTyp),
                           SchadengruppeReihung = sgm.Reihung,

                           SchadendetailTyp = sdm.SchadendetailTyp,
                           SchadendetailBezeichnung = localizationService.GetLocalizedEnum(sdm.SchadendetailTyp),
                           SchadendetailReihung = sdm.Reihung,

                           Gewicht = sgm.Gewicht,
                           Bewertung = GetMatrix(schadendetail) * sgm.Gewicht,
                           Matrix = GetMatrix(schadendetail),

                           SchadenschwereS1 = GetSchadenschwere(schadendetail, SchadenschwereTyp.S1),
                           SchadenschwereS2 = GetSchadenschwere(schadendetail, SchadenschwereTyp.S2),
                           SchadenschwereS3 = GetSchadenschwere(schadendetail, SchadenschwereTyp.S3),

                           SchadenausmassA0 = GetSchadenausmass(schadendetail, SchadenausmassTyp.A0),
                           SchadenausmassA1 = GetSchadenausmass(schadendetail, SchadenausmassTyp.A1),
                           SchadenausmassA2 = GetSchadenausmass(schadendetail, SchadenausmassTyp.A2),
                           SchadenausmassA3 = GetSchadenausmass(schadendetail, SchadenausmassTyp.A3)
                       };
        }

        private static int GetMatrix(Schadendetail schadendetail)
        {
            return schadendetail != null ? (int)schadendetail.SchadenschwereTyp * (int)schadendetail.SchadenausmassTyp : 0;
        }

        private static string GetSchadenausmass(Schadendetail schadendetail, SchadenausmassTyp schadenausmassTyp)
        {
            if (schadendetail == null)
                return string.Empty;

            return schadendetail.SchadenausmassTyp == schadenausmassTyp ? "X" : string.Empty;
        }

        private static string GetSchadenschwere(Schadendetail schadendetail, SchadenschwereTyp schadenschwereTyp)
        {
            if (schadendetail == null)
                return string.Empty;

            return schadendetail.SchadenschwereTyp == schadenschwereTyp ? "X" : string.Empty;
        }

        protected override void SetReportParameters(AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter parameter)
        {
            base.SetReportParameters(parameter);

            AddReportParameter("IsEmpty", false);
            AddReportParameter("TableHeaderBackgroundColor", "#DCE6F1");
            AddReportParameter("TableFooterBackgroundColor", "#F2F2F2");
            AddReportParameter("TableAlternatingRowBackgroungColor", "#EBF5F5");
            AddReportParameter("TableHorizontalBorderColor", "#587BA5");
            AddReportParameter("TableVerticalBorderColor", "#C2D3E8");

        }

        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }

        protected override void BuildFilterList(IFilterListBuilder<AusgefuellteErfassungsformulareFuerOberflaechenschaedenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.AufnahmedatumVon);
            filterListBuilder.AddFilterListItem(p => p.AufnahmedatumBis);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexVon);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexBis);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.Inspektionsroutename);
        }
    }
}
