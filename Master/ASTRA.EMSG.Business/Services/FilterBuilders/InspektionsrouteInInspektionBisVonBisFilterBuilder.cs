using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IInspektionsrouteInInspektionBisVonBisFilter : IFilterParameter
    {
        DateTime? InspektionsrouteInInspektionBisVon { get; }
        DateTime? InspektionsrouteInInspektionBisBis { get; }
    }

    public class InspektionsrouteInInspektionBisVonBisFilterBuilder :
        FilterBuilderBase<IInspektionsrouteInInspektionBisVonBisFilter>,
        ICanBuildFilterFor<StrassenabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.InspektionsrouteInInspektionBisVon.HasValue || Parameter.InspektionsrouteInInspektionBisBis.HasValue; }
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            if (Parameter.InspektionsrouteInInspektionBisVon.HasValue)
                source = source.Where(sa => sa.InspektionsRtStrAbschnitte.Any(ir => ir.InspektionsRouteGIS.InInspektionBis >= Parameter.InspektionsrouteInInspektionBisVon.Value));

            if (Parameter.InspektionsrouteInInspektionBisBis.HasValue)
                source = source.Where(sa => sa.InspektionsRtStrAbschnitte.Any(ir => ir.InspektionsRouteGIS.InInspektionBis <= Parameter.InspektionsrouteInInspektionBisBis.Value));

            return source;
        }
    }
}