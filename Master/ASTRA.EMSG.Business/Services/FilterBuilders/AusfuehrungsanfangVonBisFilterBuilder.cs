using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IAusfuehrungsanfangVonBisFilter : IFilterParameter
    {
        DateTime? AusfuehrungsanfangVon { get; }
        DateTime? AusfuehrungsanfangBis { get; }
    }

    public class AusfuehrungsanfangVonBisFilterBuilder :
        FilterBuilderBase<IAusfuehrungsanfangVonBisFilter>,
        ICanBuildQueryOver<KoordinierteMassnahmeGIS>
    {
        public bool ShouldFilter
        {
            get { return Parameter.AusfuehrungsanfangVon.HasValue || Parameter.AusfuehrungsanfangBis.HasValue; }
        }

        public IQueryable<KoordinierteMassnahmeGIS> BuildFilter(IQueryable<KoordinierteMassnahmeGIS> source)
        {
            if (Parameter.AusfuehrungsanfangVon.HasValue)
                source = source.Where(km => km.AusfuehrungsAnfang >= Parameter.AusfuehrungsanfangVon.Value);

            if (Parameter.AusfuehrungsanfangBis.HasValue)
                source = source.Where(km => km.AusfuehrungsAnfang <= Parameter.AusfuehrungsanfangBis.Value);

            return source;
        }

        public IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> BuildFilter(IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> source)
        {
            if (Parameter.AusfuehrungsanfangVon.HasValue)
                source = source.Where(km => km.AusfuehrungsAnfang >= Parameter.AusfuehrungsanfangVon.Value);

            if (Parameter.AusfuehrungsanfangBis.HasValue)
                source = source.Where(km => km.AusfuehrungsAnfang <= Parameter.AusfuehrungsanfangBis.Value);

            return source;
        }
    }
}