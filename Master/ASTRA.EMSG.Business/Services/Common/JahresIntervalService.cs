using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface IJahresIntervalService : IService
    {
        JahresInterval CalculateFromErfassungsPeriodList(ErfassungsPeriod currentPeriod, List<ErfassungsPeriod> erfassungsPeriodList);
    }

    public class JahresIntervalService : IJahresIntervalService
    {
        private readonly ITimeService timeService;

        public JahresIntervalService(ITimeService timeService)
        {
            this.timeService = timeService;
        }

        public JahresInterval CalculateFromErfassungsPeriodList(ErfassungsPeriod currentPeriod, List<ErfassungsPeriod> erfassungsPeriodList)
        {
            var index = erfassungsPeriodList.IndexOf(currentPeriod);

            var erfassungsJahrVon = currentPeriod.IsClosed ? currentPeriod.Erfassungsjahr.Year : Math.Max(timeService.Now.Year, currentPeriod.Erfassungsjahr.Year);
            int erfassungsJahrBis;
            if (currentPeriod == erfassungsPeriodList.Last())
                if (!currentPeriod.IsClosed)
                    erfassungsJahrBis = Math.Max(timeService.Now.Year, currentPeriod.Erfassungsjahr.Year);
                else
                    erfassungsJahrBis = erfassungsJahrVon;
            else
            {
                erfassungsJahrBis = erfassungsPeriodList[index + 1].Erfassungsjahr.Year - 1;
            }

            return new JahresInterval(erfassungsJahrVon, erfassungsJahrBis);
        }
    }
}