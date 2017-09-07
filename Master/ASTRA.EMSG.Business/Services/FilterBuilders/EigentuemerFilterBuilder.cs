using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Enums;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IEigentuemerFilter : IFilterParameter
    {
        EigentuemerTyp? Eigentuemer { get; }
    }

    public class EigentuemerFilterBuilder :
        FilterBuilderBase<IEigentuemerFilter>,
        ICanBuildQueryOver<Strassenabschnitt>,
        ICanBuildQueryOver<StrassenabschnittGIS>,
        ICanBuildFilterFor<Zustandsabschnitt>,
        ICanBuildQueryOver<ZustandsabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Eigentuemer.HasValue; }
        }

        public IQueryable<Strassenabschnitt> BuildFilter(IQueryable<Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<Strassenabschnitt, Strassenabschnitt> BuildFilter(IQueryOver<Strassenabschnitt, Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> BuildFilter(IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            StrassenabschnittGIS sa = null;
            return source.Where(z => sa.Strasseneigentuemer == Parameter.Eigentuemer.Value);
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return source.Where(z => z.Strassenabschnitt.Strasseneigentuemer == Parameter.Eigentuemer.Value);
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return source.Where(z => z.StrassenabschnittGIS.Strasseneigentuemer == Parameter.Eigentuemer.Value);
        }

        private IQueryable<T> BuildStrassenabschnittBaseFilter<T>(IQueryable<T> source) where T : StrassenabschnittBase
        {
            return source.Where(s => s.Strasseneigentuemer == Parameter.Eigentuemer.Value);
        }

        private IQueryOver<T, T> BuildStrassenabschnittBaseFilter<T>(IQueryOver<T, T> source) where T : StrassenabschnittBase
        {
            return source.Where(s => s.Strasseneigentuemer == Parameter.Eigentuemer.Value);
        }
    }
}