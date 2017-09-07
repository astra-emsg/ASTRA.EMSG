using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IInspektionsroutenameFilter : IFilterParameter
    {
        string Inspektionsroutename { get; }
    }

    public class InspektionsroutenameFilterBuilder :
        FilterBuilderBase<IInspektionsroutenameFilter>,
        ICanBuildFilterFor<ZustandsabschnittGIS>,
        ICanBuildFilterFor<StrassenabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Inspektionsroutename.HasText(); }
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return source.Where(sa => sa.InspektionsRtStrAbschnitte.Any(s => s.InspektionsRouteGIS.Bezeichnung.ToLower().Contains(Parameter.Inspektionsroutename.ToLower())));
        }
        
        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return source.Where(za => za.StrassenabschnittGIS.InspektionsRtStrAbschnitte.Any(s => s.InspektionsRouteGIS.Bezeichnung.ToLower().Contains(Parameter.Inspektionsroutename.ToLower())));
        }
    }
}