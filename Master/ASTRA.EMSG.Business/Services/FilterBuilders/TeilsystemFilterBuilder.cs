using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Enums;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface ITeilsystemFilter : IFilterParameter
    {
        TeilsystemTyp? Teilsystem { get; }
    }

    public class TeilsystemFilterBuilder :
       FilterBuilderBase<ITeilsystemFilter>,
       ICanBuildQueryOver<MassnahmenvorschlagTeilsystemeGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Teilsystem.HasValue; }
        }

        public IQueryable<MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryable<MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Teilsystem == Parameter.Teilsystem.Value);
        }

        public IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Teilsystem == Parameter.Teilsystem.Value);
        }
    }
}