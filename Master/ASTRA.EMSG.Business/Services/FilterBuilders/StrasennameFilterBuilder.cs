using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Utils;
using NHibernate;
using NHibernate.Criterion;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IStrassennameFilter : IFilterParameter
    {
        string Strassenname { get; }
    }

    public class StrasennameFilterBuilder :
        FilterBuilderBase<IStrassennameFilter>,
        ICanBuildQueryOver<Strassenabschnitt>,
        ICanBuildQueryOver<StrassenabschnittGIS>,
        ICanBuildFilterFor<Zustandsabschnitt>,
        ICanBuildQueryOver<ZustandsabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Strassenname.HasText(); }
        }

        public IQueryable<Strassenabschnitt> BuildFilter(IQueryable<Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> BuildFilter(IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<Strassenabschnitt, Strassenabschnitt> BuildFilter(IQueryOver<Strassenabschnitt, Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return source.Where(s => s.Strassenabschnitt.Strassenname.ToLower().Contains(Parameter.Strassenname.ToLower()));
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return source.Where(s => s.StrassenabschnittGIS.Strassenname.ToLower().Contains(Parameter.Strassenname.ToLower()));
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            StrassenabschnittGIS sa = null;
            return source.Where(Restrictions.On(() => sa.Strassenname).IsInsensitiveLike(Parameter.Strassenname.ToLower(), MatchMode.Anywhere));
        }

        private IQueryOver<T, T> BuildStrassenabschnittBaseFilter<T>(IQueryOver<T, T> source) where T : StrassenabschnittBase
        {
            return source.Where(Restrictions.On<T>(sa => sa.Strassenname).IsInsensitiveLike(Parameter.Strassenname.ToLower(), MatchMode.Anywhere));
        }

        private IQueryable<T> BuildStrassenabschnittBaseFilter<T>(IQueryable<T> source) where T : StrassenabschnittBase
        {
            return source.Where(s => s.Strassenname.ToLower().Contains(Parameter.Strassenname.ToLower()));
        }
    }
}
