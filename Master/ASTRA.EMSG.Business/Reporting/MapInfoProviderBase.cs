using System.Drawing;
using System.Web;
using ASTRA.EMSG.Business.Entities.Common;
using NHibernate;
using ASTRA.EMSG.Business.Services.GIS.WMS;
using ASTRA.EMSG.Business.Entities.GIS;
using GeoAPI.Geometries;
using ASTRA.EMSG.Common;
using NHibernate.Spatial.Criterion;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.GIS.WMS.WMSObjects;
using System.IO;
using System.Collections.Generic;
using System;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using System.Linq.Expressions;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IBoundingBoxFiltererBase<TReportParameter, TEntity>
        where TEntity : Entity
        where TReportParameter : EmsgGisReportParameter
    {
        IQueryOver<TEntity, TEntity> FilterForBoundingBox(IQueryOver<TEntity, TEntity> queryOver, TReportParameter parameter);
    }

    public interface IMapInfoProviderBase<TMapReportParameter>
        where TMapReportParameter : EmsgGisReportParameter
    {
        string GetMapImageInfo(TMapReportParameter parameter);
    }

    public interface IMapProviderBase<TReportParameter, TMapReportParameter, TEntity>
        where TEntity : Entity
        where TReportParameter : EmsgGisReportParameter
        where TMapReportParameter : TReportParameter
    {
        string GetMapImageInfo(TMapReportParameter parameter);
        IQueryOver<TEntity, TEntity> FilterForBoundingBox(IQueryOver<TEntity, TEntity> queryOver, TReportParameter parameter);
    }

    public abstract class MapInfoProviderBase<TReportParameter, TMapReportParameter, TEntity> : IMapProviderBase<TReportParameter, TMapReportParameter, TEntity>
        where TEntity : Entity
        where TReportParameter : EmsgGisReportParameter
        where TMapReportParameter : TReportParameter
    {
        private readonly IGISReportService gISReportService;
        protected readonly ILocalizationService localizationService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly IHistorizationService historizationService;

        public MapInfoProviderBase(IGISReportService gISReportService,
            ILocalizationService localizationService,
            IErfassungsPeriodService erfassungsPeriodService,
            IHistorizationService historizationService)
        {
            this.gISReportService = gISReportService;
            this.localizationService = localizationService;
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.historizationService = historizationService;
        }

        public string GetMapImageInfo(TMapReportParameter parameter)
        {
            parameter.Filterparams = SetFilter(parameter);
            return gISReportService.GenerateReportBitmap(parameter);
        }

        public IQueryOver<TEntity, TEntity> FilterForBoundingBox(IQueryOver<TEntity, TEntity> queryOver, TReportParameter parameter)
        {
            var filterGeom = GISService.GetGeometryFromBoundingBox(parameter.BoundingBox);
            queryOver.UnderlyingCriteria.Add(SpatialRestrictions.Filter(ExpressionHelper.GetPropertyName<StrassenabschnittGIS, IGeometry>(e => e.Shape), filterGeom));
            return queryOver;
        }
              

        private List<FilterListPo> SetFilter(TReportParameter parameter)
        {
            var filterListBuilder = new FilterListBuilder<TReportParameter>(localizationService);
            BuildFilterList(filterListBuilder);
            return filterListBuilder.GenerateFilterListPos(parameter);
        }

        private string LocalizeErfassungsPeriod(Guid? erfassungsPeriodId)
        {
            var erfp = GetErfassungsPeriod(erfassungsPeriodId);
            return erfp.IsClosed
                       ? erfp.Erfassungsjahr.Year.ToString()
                       : localizationService.GetLocalizedText("CurrentMap");
        }

        private ErfassungsPeriod GetErfassungsPeriod(Guid? erfassungsPeriodId)
        {
            if (erfassungsPeriodId == null)
                return historizationService.GetCurrentErfassungsperiod();

            return erfassungsPeriodService.GetEntityById(erfassungsPeriodId.Value);
        }
        
        protected virtual void BuildFilterList(IFilterListBuilder<TReportParameter> filterListBuilder) { }

        protected void AddErfassungsPeriodFilterListItem(IFilterListBuilder<TReportParameter> builder)
        {
            builder.AddFilterListItem(p => p.ErfassungsPeriodId,
                                      p => LocalizeErfassungsPeriod(p.ErfassungsPeriodId));
        }
    }
}
