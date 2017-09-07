using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.BusinessExceptions
{
    public class InvalidPackageVersionOldException:EmsgException
    {
        public InvalidPackageVersionOldException()
            : base(LocalizationLocator.MobileLocalization.InvalidPackageVersionOldExceptionMessage)
        { }
    }
}
