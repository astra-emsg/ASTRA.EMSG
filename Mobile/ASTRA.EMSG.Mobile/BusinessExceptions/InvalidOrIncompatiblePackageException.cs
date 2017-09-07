using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.BusinessExceptions
{
    class InvalidOrIncompatiblePackageException : EmsgException
    {
        public InvalidOrIncompatiblePackageException()
            : base(LocalizationLocator.MobileLocalization.InvalidOrIncompatiblePackageExceptionMessage)
        { }
    }

}
