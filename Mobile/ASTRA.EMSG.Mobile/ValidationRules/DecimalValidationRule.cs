using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ValidationRules
{
    public class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var valueAsString = value as string;
            if (valueAsString == null || valueAsString.Trim().Length == 0)
                return ValidationResult.ValidResult;

            if (valueAsString.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator))
                return new ValidationResult(false, LocalizationLocator.MobileLocalization.InvalidNumberValidationError);

            decimal result;
            return !decimal.TryParse(valueAsString, out result) ? new ValidationResult(false, LocalizationLocator.MobileLocalization.InvalidNumberValidationError) : ValidationResult.ValidResult;
        }
    }

    public class IntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var valueAsString = value as string;
            if (valueAsString == null || valueAsString.Trim().Length == 0)
                return ValidationResult.ValidResult;

            if (valueAsString.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator))
                return new ValidationResult(false, LocalizationLocator.MobileLocalization.InvalidNumberValidationError);

            int result;
            return !int.TryParse(valueAsString, out result) ? new ValidationResult(false, LocalizationLocator.MobileLocalization.InvalidNumberValidationError) : ValidationResult.ValidResult;
        }
    }
}
