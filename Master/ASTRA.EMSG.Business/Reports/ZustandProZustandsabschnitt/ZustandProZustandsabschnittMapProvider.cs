using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;

namespace ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt
{
    public interface IZustandProZustandsabschnittMapProvider : IMapInfoProviderBase<ZustandProZustandsabschnittMapParameter>, IBoundingBoxFiltererBase<ZustandProZustandsabschnittParameter, ZustandsabschnittGIS>, IService
    {
    }

    public class ZustandProZustandsabschnittMapProvider : MapInfoProviderBase<ZustandProZustandsabschnittParameter, ZustandProZustandsabschnittMapParameter, ZustandsabschnittGIS>, IZustandProZustandsabschnittMapProvider
    {
        public ZustandProZustandsabschnittMapProvider(IGISReportService gISReportService, ILocalizationService localizationService, IErfassungsPeriodService erfassungsPeriodService, IHistorizationService historizationService)
            : base(gISReportService, localizationService, erfassungsPeriodService, historizationService) { }

        protected override void BuildFilterList(IFilterListBuilder<ZustandProZustandsabschnittParameter> filterListBuilder)
        {
            base.BuildFilterList(filterListBuilder);
            AddErfassungsPeriodFilterListItem(filterListBuilder);
            filterListBuilder.AddFilterListItem(p => p.Eigentuemer);
            filterListBuilder.AddFilterListItem(p => p.Strassenname);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexVon);
            filterListBuilder.AddFilterListItem(p => p.ZustandsindexBis);
            filterListBuilder.AddFilterListItem(p => p.Ortsbezeichnung);
        }
    }
}