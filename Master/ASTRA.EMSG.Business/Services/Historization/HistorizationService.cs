using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Models;
using ASTRA.EMSG.Business.Models.Administration;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.Historization
{
    public interface IHistorizationService : IService
    {
        ErfassungsPeriod GetCurrentErfassungsperiod();
        ErfassungsPeriodModel GetCurrentErfassungsperiodModel();
        IEnumerable<ErfassungsabschlussModel> GetAvailableErfassungsabschlussen();
        List<ErfassungsPeriodModel> GetClosedErfassungsperiods();
        ErfassungsPeriod GetCurrentErfassungsperiod(Mandant mandant);
    }

    public class HistorizationService : IHistorizationService
    {
        private readonly IErfassungsPeriodService erfassungsPeriodService;
        private readonly ITimeService timeService;

        public HistorizationService(IErfassungsPeriodService erfassungsPeriodService, ITimeService timeService)
        {
            this.erfassungsPeriodService = erfassungsPeriodService;
            this.timeService = timeService;
        }

        public ErfassungsPeriod GetCurrentErfassungsperiod()
        {
            return erfassungsPeriodService.GetCurrentErfassungsPeriod();
        }

        public ErfassungsPeriodModel GetCurrentErfassungsperiodModel()
        {
            return  erfassungsPeriodService.GetCurrentErfassungsPeriodModel();
        }

        public ErfassungsPeriod GetCurrentErfassungsperiod(Mandant mandant)
        {
            return erfassungsPeriodService.GetCurrentErfassungsPeriod(mandant);
        }

        public IEnumerable<ErfassungsabschlussModel> GetAvailableErfassungsabschlussen()
        {
            var currentYear = timeService.Now.Year;
            var tenYearsBefore = currentYear - 9;
            var closedYears = erfassungsPeriodService.GetClosedErfassungsPeriodModels();
            var lastClosedYear = closedYears.Count > 0 ? closedYears.Max(ep => ep.Erfassungsjahr).Year : 0;
            var firstAvailableYear = Math.Max(tenYearsBefore, lastClosedYear + 1);

            return Enumerable.Range(firstAvailableYear, (currentYear - firstAvailableYear) + 1)
                .Select(year => new ErfassungsabschlussModel { AbschlussDate = new DateTime(year, 1, 1) }).ToList();

        }

        public List<ErfassungsPeriodModel> GetClosedErfassungsperiods()
        {
            return erfassungsPeriodService.GetClosedErfassungsPeriodModels();
        }
    }
}
