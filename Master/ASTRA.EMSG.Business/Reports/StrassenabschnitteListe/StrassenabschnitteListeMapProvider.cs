using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Business.Services.GIS;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListe
{
    public interface IStrassenabschnitteListeMapProvider : IMapInfoProviderBase<StrassenabschnitteListeMapParameter>, IBoundingBoxFiltererBase<StrassenabschnitteListeParameter, StrassenabschnittGIS>, IService
    {
    }

    public class StrassenabschnitteListeMapProvider : MapInfoProviderBase<StrassenabschnitteListeParameter, StrassenabschnitteListeMapParameter, StrassenabschnittGIS>, IStrassenabschnitteListeMapProvider
    {
        private readonly IBelastungskategorieService belastungskategorieService;

        public StrassenabschnitteListeMapProvider(IGISReportService gISReportService, ILocalizationService localizationService, IErfassungsPeriodService erfassungsPeriodService, IHistorizationService historizationService, IBelastungskategorieService belastungskategorieService)
            : base(gISReportService, localizationService, erfassungsPeriodService, historizationService) 
        {
            this.belastungskategorieService = belastungskategorieService;
        }


        protected override void BuildFilterList(IFilterListBuilder<StrassenabschnitteListeParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Belastungskategorie, GetLocalizedBelastungskategorieTyp);
            filterListBuilder.AddFilterListItem(p => p.Ortsbezeichnung);
        }

        protected string GetLocalizedBelastungskategorieTyp(IBelastungskategorieFilter belastungskategorieFilter)
        {
            if (!belastungskategorieFilter.Belastungskategorie.HasValue)
                return null;
            return localizationService.GetLocalizedBelastungskategorieTyp(belastungskategorieService.GetBelastungskategorie(belastungskategorieFilter.Belastungskategorie).Typ);
        }
    }
}