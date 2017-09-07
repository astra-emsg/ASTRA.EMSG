using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Master.ConfigurationHandling;
using GeoAPI.Geometries;
using NHibernate;
using NHibernate.Spatial.Criterion;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;

namespace ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen
{
    public interface IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider : IMapInfoProviderBase<EineListeVonMassnahmenGegliedertNachTeilsystemenMapParameter>, IBoundingBoxFiltererBase<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter, MassnahmenvorschlagTeilsystemeGIS>, IService
    {
    }

    public class EineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider : MapInfoProviderBase<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter,EineListeVonMassnahmenGegliedertNachTeilsystemenMapParameter, MassnahmenvorschlagTeilsystemeGIS>, IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider
    {
        public EineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider(IGISReportService gISReportService, ILocalizationService localizationService, IErfassungsPeriodService erfassungsPeriodService, IHistorizationService historizationService)
            : base(gISReportService, localizationService, erfassungsPeriodService,  historizationService) { }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Status);
            filterListBuilder.AddFilterListItem(p => p.Dringlichkeit);
            filterListBuilder.AddFilterListItem(p => p.Teilsystem);
            filterListBuilder.AddFilterListItem(p => p.Projektname);
        }
    }
}
