using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Services.Security;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface ICurrentMandantFilter : IFilterParameter
    {

    }

    public class CurrentMandantFilterBuilder  : 
        FilterBuilderBase<ICurrentMandantFilter>,
        ICanBuildFilterFor<Strassenabschnitt>,
        ICanBuildFilterFor<Zustandsabschnitt>,
        ICanBuildFilterFor<RealisierteMassnahmeSummarsich>,
        ICanBuildFilterFor<RealisierteMassnahme>,
        ICanBuildFilterFor<RealisierteMassnahmeGIS>,
        ICanBuildQueryOver<StrassenabschnittGIS>,
        ICanBuildQueryOver<ZustandsabschnittGIS>,
        ICanBuildQueryOver<KoordinierteMassnahmeGIS>,
        ICanBuildQueryOver<MassnahmenvorschlagTeilsystemeGIS>

    {
        private readonly ISecurityService securityService;

        public CurrentMandantFilterBuilder(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        public bool ShouldFilter
        {
            get { return true; }
        }

        public IQueryable<Strassenabschnitt> BuildFilter(IQueryable<Strassenabschnitt> source)
        {
            return source.Where(s => s.ErfassungsPeriod.Mandant == CurrentMandant);
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return source.Where(s => s.ErfassungsPeriod.Mandant == CurrentMandant);
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return source.Where(s => s.Strassenabschnitt.Mandant == CurrentMandant);
        }

        public IQueryable<RealisierteMassnahmeSummarsich> BuildFilter(IQueryable<RealisierteMassnahmeSummarsich> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryable<RealisierteMassnahme> BuildFilter(IQueryable<RealisierteMassnahme> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryable<RealisierteMassnahmeGIS> BuildFilter(IQueryable<RealisierteMassnahmeGIS> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryable<KoordinierteMassnahmeGIS> BuildFilter(IQueryable<KoordinierteMassnahmeGIS> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryable<MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryable<MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return source.Where(s => s.StrassenabschnittGIS.Mandant == CurrentMandant);
        }

        public IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> BuildFilter(IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            StrassenabschnittGIS sa = null;
            return source.Where(() => sa.Mandant == CurrentMandant);
        }

        public IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> BuildFilter(IQueryOver<KoordinierteMassnahmeGIS, KoordinierteMassnahmeGIS> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        public IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> BuildFilter(IQueryOver<MassnahmenvorschlagTeilsystemeGIS, MassnahmenvorschlagTeilsystemeGIS> source)
        {
            return source.Where(s => s.Mandant == CurrentMandant);
        }

        private Mandant CurrentMandant
        {
            get { return securityService.GetCurrentMandant(); }
        }
    }
}