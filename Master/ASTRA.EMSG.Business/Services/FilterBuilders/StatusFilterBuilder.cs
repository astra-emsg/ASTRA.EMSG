using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Enums;
using NHibernate;
using ASTRA.EMSG.Common.Utils;
using NHibernate.Criterion;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IStatusFilter : IFilterParameter
    {
        StatusTyp? Status { get; }
    }

    public class StatusFilterBuilder : 
        FilterBuilderBase<IStatusFilter>, 
        ICanBuildQueryOver<KoordinierteMassnahmeGIS>,
        ICanBuildQueryOver<MassnahmenvorschlagTeilsystemeGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Status.HasValue; }
        }

        public IQueryable<KoordinierteMassnahmeGIS> BuildFilter(IQueryable<KoordinierteMassnahmeGIS> source)
        {
            return source.Where(km => km.Status == Parameter.Status.Value);
        }

        public IQueryable<MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryable<MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Status == Parameter.Status.Value);
        }

        public IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> BuildFilter(IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> source)
        {
            return source.Where(km => km.Status == Parameter.Status.Value);
        }

        public IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Status == Parameter.Status.Value);
        }
    }
}