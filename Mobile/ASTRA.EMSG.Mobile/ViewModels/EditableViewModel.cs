using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class EditableViewModel : ViewModel, IDataErrorInfo
    {
        protected static MobileLocalization MobileLocalization { get { return LocalizationLocator.MobileLocalization; } }

        public EditableViewModel()
        {
            IsChildsValid = true;
            Notify(() => IsValid);
        }

        protected string DateTimeValidationMessage()
        {
            return string.Format(MobileLocalization.RangeValidationError, minDateTime.ToShortDateString(), maxDateTime.ToShortDateString());
        }

        protected DateTime minDateTime = new DateTime(1900, 01, 01);
        protected DateTime maxDateTime = new DateTime(9999, 12, 31);

        protected bool DateTimeValidator(DateTime? date)
        {
            return !date.HasValue || (date.Value > minDateTime && date.Value < maxDateTime);
        }

        protected string RangeValidationMessage(int min = 0, int max = int.MaxValue)
        {
            return string.Format(MobileLocalization.RangeValidationError, min, max);
        }

        protected string LengthValidationMessage(int max = 150)
        {
            return string.Format(MobileLocalization.LengthValidationError, max);
        }

        protected bool RequiredValidator<T>(T value) where T : class
        {
            return value != null;
        }

        protected bool RequiredValidator(decimal? d)
        {
            return d.HasValue;
        }

        protected bool RequiredValidator(DateTime? dateTime)
        {
            return dateTime.HasValue;
        }

        protected bool RequiredValidator(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        protected bool RangeValidator(decimal? number, int min = 0, int max = int.MaxValue)
        {
            return !number.HasValue || (number >= min && number <= max);
        }

        protected bool LenghtValidator(string bezeichnungBis, int length = 150)
        {
            return string.IsNullOrEmpty(bezeichnungBis) || bezeichnungBis.Length < length;
        }

        private bool triggerNotify = true;
        public string this[string columnName]
        {
            get
            {
                Notify(() => IsValid);

                if (triggerNotify)
                    Notify(() => ValidationErrorStrings);
                triggerNotify = true;

                if (validatorDictionary.ContainsKey(columnName))
                {
                    List<ICustomValidator> customValidators = validatorDictionary[columnName];
                    var invalids = customValidators.Where(v => !v.IsValid).ToList();

                    if (invalids.Any())
                        return string.Join(", ", invalids.Select(v => v.ValidationMessage));
                }

                return string.Empty;
            }
        }

        public IDataErrorInfo ValidationErrorStrings
        {
            get
            {
                triggerNotify = false;
                return this;
            }
        }

        public virtual bool IsValid { get { return !validatorDictionary.Any(vl => vl.Value.Any(v => !v.IsValid)) && IsChildsValid; } }

        public virtual void RefreshValidation()
        {
            Notify(() => IsValid);
        }

        protected bool IsChildsValid
        {
            get { return isChildsValid; }
            set { isChildsValid = value; Notify(() => IsChildsValid); Notify(() => IsValid); }
        }

        protected Dictionary<string, List<ICustomValidator>> validatorDictionary = new Dictionary<string, List<ICustomValidator>>();
        private bool isChildsValid;

        protected void RegisterValidationGeneric<TViewModel, TProperty>(Expression<Func<TViewModel, TProperty>> property, Func<bool> isValidMethod, string validationMessage)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property);
            var customValidator = new CustomValidator<TViewModel, TProperty>(property, validationMessage, isValidMethod);

            if (!validatorDictionary.ContainsKey(propertyName))
                validatorDictionary.Add(propertyName, new List<ICustomValidator> { customValidator });
            else
                validatorDictionary[propertyName].Add(customValidator);
        }

        public string Error
        {
            get { return string.Empty; }
        }
    }
}