using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IBelastungskategorieFilter : IFilterParameter
    {
        Guid? Belastungskategorie { get; set; }
    }

    public class BelastungskategorieFilterBuilder :
        FilterBuilderBase<IBelastungskategorieFilter>,
        ICanBuildQueryOver<StrassenabschnittGIS>,
        ICanBuildFilterFor<Strassenabschnitt>
    {
        public bool ShouldFilter
        {
            get { return Parameter.Belastungskategorie.HasValue; }
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return BuildStrassenabschnittBaseFilter(source);            
        }

        public IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> BuildFilter(IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryable<Strassenabschnitt> BuildFilter(IQueryable<Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryOver<Strassenabschnitt, Strassenabschnitt> BuildFilter(IQueryOver<Strassenabschnitt, Strassenabschnitt> source)
        {
            return BuildStrassenabschnittBaseFilter(source);
        }

        public IQueryable<TEntity> BuildStrassenabschnittBaseFilter<TEntity>(IQueryable<TEntity> source) where TEntity : StrassenabschnittBase
        {
            return source.Where(s => s.Belastungskategorie.Id == Parameter.Belastungskategorie);
        }

        public IQueryOver<TEntity, TEntity> BuildStrassenabschnittBaseFilter<TEntity>(IQueryOver<TEntity, TEntity> source) where TEntity : StrassenabschnittBase
        {
            return source.Where(s => s.Belastungskategorie.Id == Parameter.Belastungskategorie);
        }
    }
}