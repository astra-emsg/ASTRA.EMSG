using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.EntityServices.Strassennamen;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.WiederbeschaffungswertUndWertverlustGrafische
{
    public interface IWiederbeschaffungswertUndWertverlustGrafischePoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W2_3)]
    public class WiederbeschaffungswertUndWertverlustGrafischePoProvider : EmsgModeDependentPoProviderBase<WiederbeschaffungswertUndWertverlustGrafischeParameter, WiederbeschaffungswertUndWertverlustGrafischePo>, IWiederbeschaffungswertUndWertverlustGrafischePoProvider
    {
        private readonly IStrassenabschnittService strassenabschnittService;
        private readonly IStrassenabschnittGISService strassenabschnittGisService;
        private readonly IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService;
        private readonly IBelastungskategorieService belastungskategorieService;
        private readonly IReportLocalizationService reportLocalizationService;
        private readonly INetzSummarischDetailService netzSummarischDetailService;

        public WiederbeschaffungswertUndWertverlustGrafischePoProvider(
            IStrassenabschnittService strassenabschnittService,
            IStrassenabschnittGISService strassenabschnittGisService,
            IWiederbeschaffungswertKatalogService wiederbeschaffungswertKatalogService,
            IBelastungskategorieService belastungskategorieService,
            IReportLocalizationService reportLocalizationService,
            INetzSummarischDetailService netzSummarischDetailService)
        {
            this.strassenabschnittService = strassenabschnittService;
            this.strassenabschnittGisService = strassenabschnittGisService;
            this.wiederbeschaffungswertKatalogService = wiederbeschaffungswertKatalogService;
            this.belastungskategorieService = belastungskategorieService;
            this.reportLocalizationService = reportLocalizationService;
            this.netzSummarischDetailService = netzSummarischDetailService;
        }

        protected override List<WiederbeschaffungswertUndWertverlustGrafischePo> GetPresentationObjectListForSummarisch(WiederbeschaffungswertUndWertverlustGrafischeParameter parameter)
        {
            ErfassungsPeriod erfassungsPeriod = GetErfassungsPeriod(parameter.ErfassungsPeriodId);

            List<NetzSummarischDetail> netzSummarischDetailes = netzSummarischDetailService.GetEntitiesBy(erfassungsPeriod).ToList();

            var pos = new List<WiederbeschaffungswertUndWertverlustGrafischePo>();

            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                var wieder = wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(belastungskategorie, erfassungsPeriod);
                
                var netzSummarischDetail = netzSummarischDetailes.Single(nsd => nsd.Belastungskategorie.Id == belastungskategorie.Id);
                decimal wiederbeschaffungswert = netzSummarischDetail.Fahrbahnflaeche * wieder.FlaecheFahrbahn;

                pos.Add(CreateWiederbeschaffungswertUndWertverlustGrafischePo(belastungskategorie, reportLocalizationService.Wiederbeschaffungswert, wiederbeschaffungswert));
                pos.Add(CreateWiederbeschaffungswertUndWertverlustGrafischePo(belastungskategorie, reportLocalizationService.WertverlustsI, wiederbeschaffungswert * wieder.AlterungsbeiwertI));
                pos.Add(CreateWiederbeschaffungswertUndWertverlustGrafischePo(belastungskategorie, reportLocalizationService.WertverlustsII, wiederbeschaffungswert * wieder.AlterungsbeiwertII));
            }

            return pos;
        }

        protected override List<WiederbeschaffungswertUndWertverlustGrafischePo> GetPresentationObjectListForTabellarisch(WiederbeschaffungswertUndWertverlustGrafischeParameter parameter)
        {
            return GetPresentationObjectList(strassenabschnittService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId)), parameter);
        }

        protected override List<WiederbeschaffungswertUndWertverlustGrafischePo> GetPresentationObjectListForGis(WiederbeschaffungswertUndWertverlustGrafischeParameter parameter)
        {
            return GetPresentationObjectList(strassenabschnittGisService.GetEntitiesBy(GetErfassungsPeriod(parameter.ErfassungsPeriodId)), parameter);
        }

        private List<WiederbeschaffungswertUndWertverlustGrafischePo> GetPresentationObjectList(IQueryable<StrassenabschnittBase> strassenabschnitten, WiederbeschaffungswertUndWertverlustGrafischeParameter parameter)
        {
            var strassenabschnittenDictionary =
                FilterStrassenabschnittBase(strassenabschnitten, parameter)
                    .Fetch(sa => sa.Belastungskategorie)
                    .ToList()
                    .GroupBy(s => s.Belastungskategorie.Id)
                    .ToDictionary(s => s.Key, s => s.ToList());

            var pos = new List<WiederbeschaffungswertUndWertverlustGrafischePo>();

            foreach (var belastungskategorie in belastungskategorieService.AlleBelastungskategorie)
            {
                pos.Add(CreateWiederbeschaffungswertUndWertverlustGrafischePo(strassenabschnittenDictionary, belastungskategorie, reportLocalizationService.Wiederbeschaffungswert, GetWiederbeschaffungswert));
                pos.Add(CreateWiederbeschaffungswertUndWertverlustGrafischePo(strassenabschnittenDictionary, belastungskategorie, reportLocalizationService.WertverlustsI, GetWertverlustsI));
                pos.Add(CreateWiederbeschaffungswertUndWertverlustGrafischePo(strassenabschnittenDictionary, belastungskategorie, reportLocalizationService.WertverlustsII, GetWertverlustsII));
            }

            return pos;
        }

        private decimal GetWertverlustsI(StrassenabschnittBase strassenabschnittBase)
        {
            return GetWiederbeschaffungswert(strassenabschnittBase) * GetWiederbeschaffungswertKatalogModel(strassenabschnittBase).AlterungsbeiwertI / 100;
        }

        private decimal GetWertverlustsII(StrassenabschnittBase strassenabschnittBase)
        {
            return GetWiederbeschaffungswert(strassenabschnittBase) * GetWiederbeschaffungswertKatalogModel(strassenabschnittBase).AlterungsbeiwertII / 100;
        }

        private decimal GetWiederbeschaffungswert(StrassenabschnittBase strassenabschnittBase)
        {
            var wieder = GetWiederbeschaffungswertKatalogModel(strassenabschnittBase);
            if (strassenabschnittBase.HasTrottoirInformation)
                return strassenabschnittBase.FlaecheFahrbahn * wieder.FlaecheFahrbahn + wieder.FlaecheTrottoir * (strassenabschnittBase.FlaecheTrottoirLinks + strassenabschnittBase.FlaecheTrottoirRechts);

            return strassenabschnittBase.GesamtFlaeche * wieder.GesamtflaecheFahrbahn;
        }

        private WiederbeschaffungswertKatalogModel GetWiederbeschaffungswertKatalogModel(StrassenabschnittBase strassenabschnittBase)
        {
            return wiederbeschaffungswertKatalogService.GetWiederbeschaffungswertKatalogModel(strassenabschnittBase.Belastungskategorie, strassenabschnittBase.ErfassungsPeriod);
        }

        private WiederbeschaffungswertUndWertverlustGrafischePo CreateWiederbeschaffungswertUndWertverlustGrafischePo(Dictionary<Guid, List<StrassenabschnittBase>> strassenabschnittenDictionary, Belastungskategorie belastungskategorie, string bezeichnung, Func<StrassenabschnittBase, decimal> getMenge)
        {
            var po = GetWiederbeschaffungswertUndWertverlustGrafischePo(belastungskategorie, bezeichnung);

            List<StrassenabschnittBase> strassenabschnittenForBelastungskategorie;
            if (strassenabschnittenDictionary.TryGetValue(belastungskategorie.Id, out strassenabschnittenForBelastungskategorie))
                po.Menge = strassenabschnittenForBelastungskategorie.Sum(getMenge);

            return po;
        }

        private WiederbeschaffungswertUndWertverlustGrafischePo CreateWiederbeschaffungswertUndWertverlustGrafischePo(Belastungskategorie belastungskategorie, string bezeichnung, decimal menge)
        {
            var po = GetWiederbeschaffungswertUndWertverlustGrafischePo(belastungskategorie, bezeichnung);
            po.Menge = menge;
            return po;
        }

        private WiederbeschaffungswertUndWertverlustGrafischePo GetWiederbeschaffungswertUndWertverlustGrafischePo(Belastungskategorie belastungskategorie, string bezeichnung)
        {
            return new WiederbeschaffungswertUndWertverlustGrafischePo
                       {
                           Bezeichnung = bezeichnung,
                           BelastungskategorieBezeichnung = LocalizationService.GetLocalizedBelastungskategorieTyp(belastungskategorie.Typ),
                           BelastungskategorieReihenfolge = belastungskategorie.Reihenfolge,
                           ColorCode = belastungskategorie.ColorCode
                       };
        }

        private static IQueryable<T> FilterStrassenabschnittBase<T>(IQueryable<T> strassenabschnitten, WiederbeschaffungswertUndWertverlustGrafischeParameter parameter)
            where T : StrassenabschnittBase
        {
            if (parameter.Eigentuemer.HasValue)
                strassenabschnitten = strassenabschnitten.Where(s => s.Strasseneigentuemer == parameter.Eigentuemer.Value);
            return strassenabschnitten;
        }

        public override bool AvailableInCurrentErfassungPeriod { get { return true; } }

        protected override void BuildFilterList(IFilterListBuilder<WiederbeschaffungswertUndWertverlustGrafischeParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
        }

        public override int? MaxImagePreviewPageHeight { get  { return 620; } }
    }
}