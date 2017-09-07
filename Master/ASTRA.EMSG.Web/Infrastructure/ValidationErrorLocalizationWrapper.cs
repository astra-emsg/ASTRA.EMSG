using System.Resources;
using Resources;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class ValidationErrorLocalizationWrapper
    {
        public static ResourceManager ResourceManager { get { return ValidationErrorLocalization.ResourceManager; } }

        public static string email_error { get { return ValidationErrorLocalization.email_error; } }
        public static string exact_length_error { get { return ValidationErrorLocalization.exact_length_error; } }
        public static string equal_error { get { return ValidationErrorLocalization.equal_error; } }
        public static string exclusivebetween_error { get { return ValidationErrorLocalization.exclusivebetween_error; } }
        public static string greaterthan_error { get { return ValidationErrorLocalization.greaterthan_error; } }
        public static string greaterthanorequal_error { get { return ValidationErrorLocalization.greaterthanorequal_error; } }
        public static string inclusivebetween_error { get { return ValidationErrorLocalization.inclusivebetween_error; } }
        public static string length_error { get { return ValidationErrorLocalization.length_error; } }
        public static string lessthan_error { get { return ValidationErrorLocalization.lessthan_error; } }
        public static string lessthanorequal_error { get { return ValidationErrorLocalization.lessthanorequal_error; } }
        public static string notempty_error { get { return ValidationErrorLocalization.notempty_error; } }
        public static string notequal_error { get { return ValidationErrorLocalization.notequal_error; } }
        public static string notnull_error { get { return ValidationErrorLocalization.notnull_error; } }
        public static string predicate_error { get { return ValidationErrorLocalization.predicate_error; } }
        public static string regex_error { get { return ValidationErrorLocalization.regex_error; } }
    }
}