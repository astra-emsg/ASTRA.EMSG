using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustProBelastungskategorie
{
    public interface IWiederbeschaffungswertUndWertverlustProBelastungskategoriePoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W2_1)]
    public class WiederbeschaffungswertUndWertverlustProBelastungskategoriePoProvider : EmsgTablePoProviderBase<WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter, WiederbeschaffungswertUndWertverlustProBelastungskategoriePo>, IWiederbeschaffungswertUndWertverlustProBelastungskategoriePoProvider
    {
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGISService;
        private readonly IZustandsabschnittService zustandindexService;
        private readonly IZustandsabschnittGISService zustandindexGISService;
        private readonly IFiltererFactory filtererFactory;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;

        public WiederbeschaffungswertUndWertverlustProBelastungskategoriePoProvider(
            IStrassenabschnittService strassenabschnittService,
            IStrassenabschnittGISService strassenabschnittGISService,
            IZustandsabschnittService zustandindexService,
            IZustandsabschnittGISService zustandindexGISService,
            IFiltererFactory filtererFactory,
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService,
            IBelastungskategorieService belastungskategorieService,
            INetzSummarischDetailService netzSummarischDetailService
            )
        {
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGISService = strassenabschnittGISService;
            this.zustandindexService = zustandindexService;
            this.zustandindexGISService = zustandindexGISService;
            this.filtererFactory = filtererFactory;
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.belastungskategorieService = belastungskategorieService;
            this.netzSummarischDetailService = netzSummarischDetailService;
        }

        protected override List<WiederbeschaffungswertUndWertverlustProBelastungskategoriePo> GetPresentationObjectListForSummarisch(WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter parameter)
        {
            var erfassungsPeriod = GetErfassungsPeriod(parameter.ErfassungsPeriodId);
            var netzSummarischDetailList = netzSummarischDetailService.GetEntitiesBy(erfassungsPeriod).ToList();

            var pos = new List<WiederbeschaffungswertUndWertverlustProBelastungskategoriePo>();
            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                var netzSummarischDetail = netzSummarischDetailList.Single(sa => sa.Belastungskategorie.Id == belastungskategorie.Id);
                var wieder = wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(belastungskategorie, erfassungsPeriod);

                var wiederbeschaffungswert = netzSummarischDetail.Fahrbahnflaeche * wieder.GesamtflaecheFahrbahn;

                var zustandqueryable = zustandindexService.GetEntitiesBy(erfassungsPeriod);
                var zustandabschnitten = filtererFactory.CreateFilterer<Zustandsabschnitt>(parameter).Filter(zustandqueryable).Fetch(za => za.Strassenabschnitt).ThenFetch(za => za.Belastungskategorie).ToList().Cast<ZustandsabschnittBase>().ToList();

                var po = new WiederbeschaffungswertUndWertverlustProBelastungskategoriePo
                {
                    BelastungskategorieTyp = belastungskategorie.Typ,
                    BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(belastungskategorie.Typ),

                    GesamtFlaeche = netzSummarischDetail.Fahrbahnflaeche,
                    FlaecheFahrbahn = netzSummarischDetail.Fahrbahnflaeche,

                    Wiederbeschaffungswert = wiederbeschaffungswert,

                    AlterungsbeiwertI = wieder.AlterungsbeiwertI,
                    AlterungsbeiwertII = wieder.AlterungsbeiwertII,

                    WertlustI = wiederbeschaffungswert * wieder.AlterungsbeiwertI / 100,
                    WertlustII = wiederbeschaffungswert * wieder.AlterungsbeiwertII / 100,

                    MittlererZustandindex = netzSummarischDetail.MittlererZustand,
                };

                pos.Add(po);
            }

            return pos;
        }

        protected override List<WiederbeschaffungswertUndWertverlustProBelastungskategoriePo> GetPresentationObjectListForTabellarisch(WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter parameter)
        {
            var erfassungsPeriod = GetErfassungsPeriod(parameter.ErfassungsPeriodId);
            var queryable = strassenabschnittService.GetEntitiesBy(erfassungsPeriod);
            var strassenabschnitten = filtererFactory.CreateFilterer<Strassenabschnitt>(parameter).Filter(queryable).Fetch(sa => sa.Belastungskategorie).ToList().Cast<StrassenabschnittBase>().ToList();

            var zustandqueryable = zustandindexService.GetEntitiesBy(erfassungsPeriod);
            var zustandabschnitten = filtererFactory.CreateFilterer<Zustandsabschnitt>(parameter).Filter(zustandqueryable).Fetch(za => za.Strassenabschnitt).ThenFetch(sa => sa.Belastungskategorie).ToList().Cast<ZustandsabschnittBase>().ToList();
      
            return GetPos(strassenabschnitten, zustandabschnitten, erfassungsPeriod);
        }

        protected override List<WiederbeschaffungswertUndWertverlustProBelastungskategoriePo> GetPresentationObjectListForGis(WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter parameter)
        {
            var erfassungsPeriod = GetErfassungsPeriod(parameter.ErfassungsPeriodId);
            var queryable = strassenabschnittGISService.GetEntitiesBy(erfassungsPeriod);
            var strassenabschnitten = filtererFactory.CreateFilterer<StrassenabschnittGIS>(parameter).Filter(queryable).Fetch(sa => sa.Belastungskategorie).ToList().Cast<StrassenabschnittBase>().ToList();

            var zustandqueryable = zustandindexGISService.GetEntitiesBy(erfassungsPeriod);
            var zustandabschnitten = filtererFactory.CreateFilterer<ZustandsabschnittGIS>(parameter).Filter(zustandqueryable).Fetch(za => za.StrassenabschnittGIS).ThenFetch(za => za.Belastungskategorie).ToList().Cast<ZustandsabschnittBase>().ToList();
            
            return GetPos(strassenabschnitten, zustandabschnitten, erfassungsPeriod);
        }

        private List<WiederbeschaffungswertUndWertverlustProBelastungskategoriePo> GetPos(List<StrassenabschnittBase> strassenabschnittBaseList, List<ZustandsabschnittBase> zustandsabschnittBaseList, ErfassungsPeriod erfassungsPeriod)
        {
            var pos = new List<WiederbeschaffungswertUndWertverlustProBelastungskategoriePo>();
            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                var bk = belastungskategorie;
                var strassenabschnitten = strassenabschnittBaseList.Where(sa => sa.Belastungskategorie.Id == bk.Id).ToList();

                var wieder = wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(belastungskategorie, erfassungsPeriod);

                var wiederProStrassenabschnitt =
                    strassenabschnitten.Select(sa =>
                                               {
                                                   var wiederbeschaffungswert = GetWiederbeschaffungswert(sa, wieder);
                                                   return new
                                                              {
                                                                  Wiederbeschaffungswert = wiederbeschaffungswert,
                                                                  WertlustI = wiederbeschaffungswert * wieder.AlterungsbeiwertI / 100,
                                                                  WertlustII = wiederbeschaffungswert * wieder.AlterungsbeiwertII / 100
                                                              };
                                               })
                                               .ToList();
                var zustandabschniten =
                    zustandsabschnittBaseList.Where(za => za.Belastungskategorie.Id == bk.Id).ToList();

                var totalFlaeche = zustandabschniten.Sum(za => za.FlaecheFahrbahn.HasValue ? za.FlaecheFahrbahn.Value : 0);

                decimal? mittlererZustandIndex = totalFlaeche == 0 ? null :
                    zustandabschniten.Where(za => za.FlaecheFahrbahn.HasValue).Sum(za => za.FlaecheFahrbahn * za.Zustandsindex / totalFlaeche);

                var po = new WiederbeschaffungswertUndWertverlustProBelastungskategoriePo
                             {
                                 BelastungskategorieTyp = bk.Typ,
                                 BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(bk.Typ),

                                 FlaecheFahrbahn = Math.Round(strassenabschnitten.Sum(sa => sa.FlaecheFahrbahn)),
                                 FlaecheTrottoir = strassenabschnitten.Sum(sa => sa.FlaecheTrottoir),

                                 Wiederbeschaffungswert = wiederProStrassenabschnitt.Sum(w => w.Wiederbeschaffungswert),

                                 AlterungsbeiwertI = wieder.AlterungsbeiwertI,
                                 AlterungsbeiwertII = wieder.AlterungsbeiwertII,

                                 WertlustI = wiederProStrassenabschnitt.Sum(w => w.WertlustI),
                                 WertlustII = wiederProStrassenabschnitt.Sum(w => w.WertlustII),

                                 MittlererZustandindex = mittlererZustandIndex

                             };

                pos.Add(po);
            }

            return pos;
        }

        private decimal GetWiederbeschaffungswert(StrassenabschnittBase strassenabschnittBase, WiederbeschaffungswertKatalogModel wieder)
        {
            if (strassenabschnittBase.HasTrottoirInformation)
                return strassenabschnittBase.FlaecheFahrbahn * wieder.FlaecheFahrbahn + wieder.FlaecheTrottoir * strassenabschnittBase.FlaecheTrottoir;

            return strassenabschnittBase.GesamtFlaeche * wieder.GesamtflaecheFahrbahn;
        }

        protected override void SetReportParameters(WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter parameter)
        {
            base.SetReportParameters(parameter);

            var ep = GetErfassungsPeriod(parameter);

            AddReportParameter("IsSummarischeModus", ep.NetzErfassungsmodus == NetzErfassungsmodus.Summarisch);
        }

        protected override void BuildFilterList(IFilterListBuilder<WiederbeschaffungswertUndWertverlustProBelastungskategorieParameter> builder)
        {
            base.BuildFilterList(builder);
            AddErfassungsPeriodFilterListItem(builder);
            builder.AddFilterListItem(p => p.Eigentuemer);
        }

        protected override PaperType PaperType { get { return PaperType.A4Landscape; } }
    }
}