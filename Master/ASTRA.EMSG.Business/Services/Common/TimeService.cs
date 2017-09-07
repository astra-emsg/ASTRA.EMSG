using System;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface ITimeService : IService
    {
        DateTime Now { get; }
    }

    public class TimeService  : ITimeService
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}