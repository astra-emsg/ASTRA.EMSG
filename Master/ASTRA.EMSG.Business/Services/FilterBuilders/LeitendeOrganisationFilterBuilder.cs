using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface ILeitendeOrganisation : IFilterParameter
    {
        string LeitendeOrganisation { get; }
    }

    public class LeitendeOrganisationFilterBuilder :
        FilterBuilderBase<ILeitendeOrganisation>,
        ICanBuildFilterFor<RealisierteMassnahmeSummarsich>,
        ICanBuildFilterFor<RealisierteMassnahme>,
        ICanBuildFilterFor<RealisierteMassnahmeGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.LeitendeOrganisation.HasText();  }
        }

        public IQueryable<RealisierteMassnahmeSummarsich> BuildFilter(IQueryable<RealisierteMassnahmeSummarsich> source)
        {
            return source.Where(s => false);
        }

        public IQueryable<RealisierteMassnahme> BuildFilter(IQueryable<RealisierteMassnahme> source)
        {
            return source.Where(s => false);
        }

        public IQueryable<RealisierteMassnahmeGIS> BuildFilter(IQueryable<RealisierteMassnahmeGIS> source)
        {
            return source.Where(s => s.LeitendeOrganisation.ToLower().Contains(Parameter.LeitendeOrganisation.ToLower()));
        }
    }
}