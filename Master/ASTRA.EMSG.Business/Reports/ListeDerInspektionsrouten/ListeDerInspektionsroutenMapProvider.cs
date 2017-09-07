using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;

namespace ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten
{
    public interface IListeDerInspektionsroutenMapProvider : IMapInfoProviderBase<ListeDerInspektionsroutenMapParameter>, IBoundingBoxFiltererBase<ListeDerInspektionsroutenParameter, StrassenabschnittGIS>, IService
    {
    }

    public class ListeDerInspektionsroutenMapProvider : MapInfoProviderBase<ListeDerInspektionsroutenParameter,ListeDerInspektionsroutenMapParameter, StrassenabschnittGIS>, IListeDerInspektionsroutenMapProvider
    {

        public ListeDerInspektionsroutenMapProvider(IGISReportService gISReportService, ILocalizationService localizationService, IErfassungsPeriodService erfassungsPeriodService, IHistorizationService historizationService)
            : base(gISReportService, localizationService, erfassungsPeriodService, historizationService) { }

        protected override void BuildFilterList(IFilterListBuilder<ListeDerInspektionsroutenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Inspektionsroutename);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.InspektionsrouteInInspektionBei);
            filterListBuilder.AddFilterListItem(p => p.InspektionsrouteInInspektionBisVon);
            filterListBuilder.AddFilterListItem(p => p.InspektionsrouteInInspektionBisBis);
        }
    }
}