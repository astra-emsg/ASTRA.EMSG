using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IZustandsindexVonBisFilter : IFilterParameter
    {
        decimal? ZustandsindexVon { get; }
        decimal? ZustandsindexBis { get; }
    }

    public class ZustandsindexVonBisFilterBuilder :
        FilterBuilderBase<IZustandsindexVonBisFilter>,
        ICanBuildQueryOver<ZustandsabschnittGIS>,
        ICanBuildQueryOver<Zustandsabschnitt>
    {
        public bool ShouldFilter
        {
            get { return Parameter.ZustandsindexVon.HasValue || Parameter.ZustandsindexBis.HasValue; }
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryOver<Zustandsabschnitt, Zustandsabschnitt> BuildFilter(IQueryOver<Zustandsabschnitt, Zustandsabschnitt> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryOver<T, T> BuildZustandsabschnittBaseFilter<T>(IQueryOver<T, T> source) where T : ZustandsabschnittBase
        {
            if (Parameter.ZustandsindexVon.HasValue)
                source = source.Where(za => za.Zustandsindex >= Parameter.ZustandsindexVon.Value);

            if (Parameter.ZustandsindexBis.HasValue)
                source = source.Where(za => za.Zustandsindex <= Parameter.ZustandsindexBis.Value);

            return source;
        }

        public IQueryable<T> BuildZustandsabschnittBaseFilter<T>(IQueryable<T> source) where T : ZustandsabschnittBase
        {
            if (Parameter.ZustandsindexVon.HasValue)
                source = source.Where(za => za.Zustandsindex >= Parameter.ZustandsindexVon.Value);

            if (Parameter.ZustandsindexBis.HasValue)
                source = source.Where(za => za.Zustandsindex <= Parameter.ZustandsindexBis.Value);

            return source;
        }
    }
}