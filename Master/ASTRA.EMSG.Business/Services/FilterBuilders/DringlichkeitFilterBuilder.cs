using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Common.Enums;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IDringlichkeitFilter : IFilterParameter
    {
        DringlichkeitTyp? Dringlichkeit { get; }
    }

    public class DringlichkeitFilterBuilder : 
        FilterBuilderBase<IDringlichkeitFilter>,
        ICanBuildQueryOver<ZustandsabschnittGIS>,
        ICanBuildQueryOver<Zustandsabschnitt>,
        ICanBuildQueryOver<MassnahmenvorschlagTeilsystemeGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Dringlichkeit.HasValue; }
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryOver<Zustandsabschnitt, Zustandsabschnitt> BuildFilter(IQueryOver<Zustandsabschnitt, Zustandsabschnitt> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryable<MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryable<MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Dringlichkeit == Parameter.Dringlichkeit.Value);
        }

        public IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(km => km.Dringlichkeit == Parameter.Dringlichkeit.Value);
        }


        private IQueryable<T> BuildZustandsabschnittBaseFilter<T>(IQueryable<T> source) 
            where T : ZustandsabschnittBase
        {
            return source.Where(z => z.DringlichkeitFahrbahn == Parameter.Dringlichkeit);
        }

        private IQueryOver<T, T> BuildZustandsabschnittBaseFilter<T>(IQueryOver<T, T> source) 
            where T : ZustandsabschnittBase
        {
            return source.Where(z => z.DringlichkeitFahrbahn == Parameter.Dringlichkeit);
        }
    }
}