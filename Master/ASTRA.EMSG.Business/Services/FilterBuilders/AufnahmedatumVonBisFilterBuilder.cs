using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IAufnahmedatumVonBisFilter : IFilterParameter
    {
        DateTime? AufnahmedatumVon { get; }
        DateTime? AufnahmedatumBis { get; }
    }

    public class AufnahmedatumVonBisFilterBuilder :
        FilterBuilderBase<IAufnahmedatumVonBisFilter>,
        ICanBuildFilterFor<Zustandsabschnitt>,
        ICanBuildFilterFor<ZustandsabschnittGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.AufnahmedatumVon.HasValue || Parameter.AufnahmedatumBis.HasValue; }
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return BuildZustandsabschnittBaseFilter(source);
        }

        private IQueryable<T> BuildZustandsabschnittBaseFilter<T>(IQueryable<T> source) where T : ZustandsabschnittBase
        {
            if (Parameter.AufnahmedatumVon.HasValue)
                source = source.Where(za => za.Aufnahmedatum >= Parameter.AufnahmedatumVon.Value);

            if (Parameter.AufnahmedatumBis.HasValue)
                source = source.Where(za => za.Aufnahmedatum <= Parameter.AufnahmedatumBis.Value);

            return source;
        }
    }
}