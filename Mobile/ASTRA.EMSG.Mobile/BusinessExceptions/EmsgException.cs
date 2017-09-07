using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Mobile.BusinessExceptions
{
    public abstract class EmsgException:Exception
    {
        public EmsgException(string localizedMessage, Exception e)
            : base(localizedMessage, e)
        { }
        public EmsgException(string localizedMessage)
            : base(localizedMessage)
        { }
    }
}
