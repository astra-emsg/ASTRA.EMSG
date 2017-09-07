using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Utils;
using NHibernate;
using NHibernate.Criterion;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IProjektnameFilter : IFilterParameter
    {
        string Projektname { get; }
    }

    public class ProjektnameFilterBuilder : 
        FilterBuilderBase<IProjektnameFilter>, 
        ICanBuildFilterFor<RealisierteMassnahmeSummarsich>,
        ICanBuildFilterFor<RealisierteMassnahme>,
        ICanBuildFilterFor<RealisierteMassnahmeGIS>,
        ICanBuildQueryOver<KoordinierteMassnahmeGIS>,
        ICanBuildQueryOver<MassnahmenvorschlagTeilsystemeGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Projektname.HasText(); }
        }

        public IQueryable<RealisierteMassnahmeSummarsich> BuildFilter(IQueryable<RealisierteMassnahmeSummarsich> source)
        {
            return source.Where(s => s.Projektname.ToLower().Contains(Parameter.Projektname.ToLower()));
        }

        public IQueryable<RealisierteMassnahme> BuildFilter(IQueryable<RealisierteMassnahme> source)
        {
            return source.Where(s => s.Projektname.ToLower().Contains(Parameter.Projektname.ToLower()));
        }

        public IQueryable<RealisierteMassnahmeGIS> BuildFilter(IQueryable<RealisierteMassnahmeGIS> source)
        {
            return source.Where(s => s.Projektname.ToLower().Contains(Parameter.Projektname.ToLower()));
        }

        public IQueryable<KoordinierteMassnahmeGIS> BuildFilter(IQueryable<KoordinierteMassnahmeGIS> source)
        {
            return source.Where(km => km.Projektname.ToLower().Contains(Parameter.Projektname.ToLower()));
        }

        public IQueryable<MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryable<MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Projektname.ToLower().Contains(Parameter.Projektname.ToLower()));
        }

        public IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> BuildFilter(IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> source)
        {
            return source.Where(Restrictions.On<KoordinierteMassnahmeGIS>((km) => km.Projektname).IsInsensitiveLike(Parameter.Projektname.ToLower(), MatchMode.Anywhere));
        }

        public IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(Restrictions.On<KoordinierteMassnahmeGIS>((km) => km.Projektname).IsInsensitiveLike(Parameter.Projektname.ToLower(), MatchMode.Anywhere));
        }
    }
}