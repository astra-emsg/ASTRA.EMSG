using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IInspektionsrouteInInspektionBeiFilter : IFilterParameter
    {
        string InspektionsrouteInInspektionBei { get; }
    }

    public class InspektionsrouteInInspektionBeiFilterBuilder :
        FilterBuilderBase<IInspektionsrouteInInspektionBeiFilter>,
        ICanBuildFilterFor<StrassenabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.InspektionsrouteInInspektionBei.HasText(); }
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return source.Where(sa => sa.InspektionsRtStrAbschnitte.Any(s => s.InspektionsRouteGIS.Bezeichnung.ToLower().Contains(Parameter.InspektionsrouteInInspektionBei.ToLower())));
        }
    }
}