using System;

namespace ASTRA.EMSG.Common.Master.Logging
{
    public class ErrorLoggingContextInfo
    {
        public Type EntityType { get; set; }
        public Guid? EntityId { get; set; }
        
        public Guid? MandantId { get; set; }
        public string MandantName { get; set; }

        public Guid? ErfassungsPeriodId { get; set; }
        public string ErfassungsPeriodName { get; set; }

        public override string ToString()
        {
            try
            {
                return string.Format("ContextInfo: [Entity]: {0} (Id: {1}), [Mandant]: {2} (Id: {3}), [ErfassungsPeriod]: {4} (Id: {5})",
                    EntityType == null ? null : EntityType.Name, EntityId, MandantName, MandantId, ErfassungsPeriodName, ErfassungsPeriodId);
            }
            catch (Exception)
            {
                return "~";
            }
        }
    }
}
