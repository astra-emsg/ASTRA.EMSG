using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Utils;
using NHibernate;
using NHibernate.Criterion;
using ASTRA.EMSG.Business.Entities.Katalogs;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IOrtsbezeichnungFilter : IFilterParameter
    {
        string Ortsbezeichnung { get; }
    }

    public class OrtsbezeichnungFilterBuilder :
        FilterBuilderBase<IOrtsbezeichnungFilter>,
        ICanBuildQueryOver<Strassenabschnitt>,
        ICanBuildQueryOver<StrassenabschnittGIS>,
        ICanBuildQueryOver<Zustandsabschnitt>,
        ICanBuildQueryOver<ZustandsabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Ortsbezeichnung.HasText(); }
        }

        public IQueryable<Strassenabschnitt> BuildFilter(IQueryable<Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<Strassenabschnitt, Strassenabschnitt> BuildFilter(IQueryOver<Strassenabschnitt, Strassenabschnitt> source)
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
        
        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return source.Where(s => s.Strassenabschnitt.Ortsbezeichnung.ToLower().Contains(Parameter.Ortsbezeichnung.ToLower()));
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return source.Where(s => s.StrassenabschnittGIS.Ortsbezeichnung.ToLower().Contains(Parameter.Ortsbezeichnung.ToLower()));
        }

        public IQueryOver<Zustandsabschnitt, Zustandsabschnitt> BuildFilter(IQueryOver<Zustandsabschnitt, Zustandsabschnitt> source)
        {
            Strassenabschnitt sa = null;
            return source.Where(Restrictions.On(() => sa.Ortsbezeichnung).IsInsensitiveLike(Parameter.Ortsbezeichnung.ToLower(), MatchMode.Anywhere));
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            StrassenabschnittGIS sa = null;
            return source.Where(Restrictions.On(() => sa.Ortsbezeichnung).IsInsensitiveLike(Parameter.Ortsbezeichnung.ToLower(), MatchMode.Anywhere));
     
        }

        private IQueryable<T> BuildStrassenabschnittBaseFilter<T>(IQueryable<T> source) where T : StrassenabschnittBase
        {
            return source.Where(s => s.Ortsbezeichnung.ToLower().Contains(Parameter.Ortsbezeichnung.ToLower()));
        }

        private IQueryOver<T, T> BuildStrassenabschnittBaseFilter<T>(IQueryOver<T, T> source) where T : StrassenabschnittBase
        {
            return source.Where(Restrictions.On<T>(sa => sa.Ortsbezeichnung).IsInsensitiveLike(Parameter.Ortsbezeichnung.ToLower(), MatchMode.Anywhere));
        }
    }
}
