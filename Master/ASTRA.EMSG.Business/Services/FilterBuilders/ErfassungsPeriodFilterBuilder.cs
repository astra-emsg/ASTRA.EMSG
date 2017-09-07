using System;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Historization;
using NHibernate;

namespace ASTRA.EMSG.Business.Services.FilterBuilders
{
    public interface IErfassungsPeriodFilter : IFilterParameter
    {
        Guid? ErfassungsPeriodId { get; set; }
    }

    public class ErfassungsPeriodFilterBuilder :
        FilterBuilderBase<IErfassungsPeriodFilter>,
        ICanBuildFilterFor<Strassenabschnitt>,
        ICanBuildFilterFor<Zustandsabschnitt>,
        ICanBuildQueryOver<StrassenabschnittGIS>,
        ICanBuildQueryOver<ZustandsabschnittGIS>
    {
        private readonly IHistorizationService historizationService;
        private readonly IErfassungsPeriodService erfassungsPeriodService;

        public ErfassungsPeriodFilterBuilder(IHistorizationService historizationService, IErfassungsPeriodService erfassungsPeriodService)
        {
            this.historizationService = historizationService;
            this.erfassungsPeriodService = erfassungsPeriodService;
        }

        public bool ShouldFilter
        {
            get { return true; }
        }

        private ErfassungsPeriod GetErfassungsPeriod()
        {
            if (Parameter.ErfassungsPeriodId == null)
                return historizationService.GetCurrentErfassungsperiod();

            return erfassungsPeriodService.GetEntityById(Parameter.ErfassungsPeriodId.Value);
        }

        public IQueryable<Strassenabschnitt> BuildFilter(IQueryable<Strassenabschnitt> source)
        {
            return source.Where(s => s.ErfassungsPeriod == GetErfassungsPeriod());
        }

        public IQueryable<StrassenabschnittGIS> BuildFilter(IQueryable<StrassenabschnittGIS> source)
        {
            return source.Where(s => s.ErfassungsPeriod == GetErfassungsPeriod());
        }

        public IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> BuildFilter(IQueryOver<StrassenabschnittGIS, StrassenabschnittGIS> source)
        {
            return source.Where(s => s.ErfassungsPeriod == GetErfassungsPeriod());
        }

        public IQueryable<Zustandsabschnitt> BuildFilter(IQueryable<Zustandsabschnitt> source)
        {
            return source.Where(s => s.Strassenabschnitt.ErfassungsPeriod == GetErfassungsPeriod());
        }

        public IQueryable<ZustandsabschnittGIS> BuildFilter(IQueryable<ZustandsabschnittGIS> source)
        {
            return source.Where(s => s.StrassenabschnittGIS.ErfassungsPeriod == GetErfassungsPeriod());
        }

        public IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> BuildFilter(IQueryOver<ZustandsabschnittGIS, ZustandsabschnittGIS> source)
        {
            StrassenabschnittGIS sa = null;
            return source.Where(() => sa.ErfassungsPeriod == GetErfassungsPeriod());
        }
    }
}