using System;
using System.Linq.Expressions;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class CustomValidator<TViewModel, TProperty> : ICustomValidator
    {
        public CustomValidator(Expression<Func<TViewModel, TProperty>> property, string validationMessage, Func<bool> isValidMethod)
        {
            Property = property;
            ValidationMessage = validationMessage;
            IsValidMethod = isValidMethod;
        }

        public Expression<Func<TViewModel, TProperty>> Property { get; set; }
        public string ValidationMessage { get; private set; }
        public Func<bool> IsValidMethod { get; private set; }

        public bool IsValid { get { return IsValidMethod(); } }
    }
}