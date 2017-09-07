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
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;

namespace ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen
{
    public interface IEineListeVonKoordiniertenMassnahmenMapProvider : IMapInfoProviderBase<EineListeVonKoordiniertenMassnahmenMapParameter>, IBoundingBoxFiltererBase<EineListeVonKoordiniertenMassnahmenParameter, KoordinierteMassnahmeGIS>, IService
    {
    }

    public class EineListeVonKoordiniertenMassnahmenMapProvider : MapInfoProviderBase<EineListeVonKoordiniertenMassnahmenParameter, EineListeVonKoordiniertenMassnahmenMapParameter, KoordinierteMassnahmeGIS>, IEineListeVonKoordiniertenMassnahmenMapProvider
    {
        
        private readonly ICreateWMSRequest createWMSRequest;

        public EineListeVonKoordiniertenMassnahmenMapProvider(IGISReportService gISReportService, ILocalizationService localizationService, IErfassungsPeriodService erfassungsPeriodService, IHistorizationService historizationService, IBelastungskategorieService belastungskategorieService)
            : base(gISReportService, localizationService, erfassungsPeriodService, historizationService) { }

        protected override void BuildFilterList(IFilterListBuilder<EineListeVonKoordiniertenMassnahmenParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Projektname);
            filterListBuilder.AddFilterListItem(p => p.Status);
            filterListBuilder.AddFilterListItem(p => p.AusfuehrungsanfangVon);
            filterListBuilder.AddFilterListItem(p => p.AusfuehrungsanfangBis);
        }
    }
}