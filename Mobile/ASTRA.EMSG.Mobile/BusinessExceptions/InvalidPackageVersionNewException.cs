using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Localization;


namespace ASTRA.EMSG.Mobile.BusinessExceptions
{
    public class InvalidPackageVersionNewException:EmsgException
    {
        public InvalidPackageVersionNewException()
            : base(LocalizationLocator.MobileLocalization.InvalidPackageVersionNewExceptionMessage)
        { }
    }
}
