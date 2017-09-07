using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt;
using ASTRA.EMSG.Business.Services;
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

namespace ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt
{
    public interface IMassnahmenvorschlagProZustandsabschnittMapProvider : IMapInfoProviderBase<MassnahmenvorschlagProZustandsabschnittMapParameter>, IBoundingBoxFiltererBase<MassnahmenvorschlagProZustandsabschnittParameter, ZustandsabschnittGIS>, IService
    {
    }

    public class MassnahmenvorschlagProZustandsabschnittMapProvider : MapInfoProviderBase<MassnahmenvorschlagProZustandsabschnittParameter, MassnahmenvorschlagProZustandsabschnittMapParameter, ZustandsabschnittGIS>, IMassnahmenvorschlagProZustandsabschnittMapProvider
    {
        public MassnahmenvorschlagProZustandsabschnittMapProvider(IGISReportService gISReportService, ILocalizationService localizationService, IErfassungsPeriodService erfassungsPeriodService, IHistorizationService historizationService)
            : base(gISReportService, localizationService, erfassungsPeriodService, historizationService) { }

        protected override void BuildFilterList(IFilterListBuilder<MassnahmenvorschlagProZustandsabschnittParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Dringlichkeit);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexVon);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexBis);
            filterListBuilder.AddFilterListItem(p => p.Ortsbezeichnung);
        }

    }
}