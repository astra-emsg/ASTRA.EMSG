using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Exceptions;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.AchsenUpdate
{
    public class AchsenUpdateAbschnittLockedException : EmsgException
    {
        public AchsenUpdateAbschnittLockedException() : base(EmsgExceptionType.UpdateAbschnittLocked) { }
    }
}
