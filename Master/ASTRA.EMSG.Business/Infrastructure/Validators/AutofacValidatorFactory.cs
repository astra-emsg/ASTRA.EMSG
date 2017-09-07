using System;
using Autofac.Features.Indexed;
using FluentValidation;

namespace ASTRA.EMSG.Business.Infrastructure.Validators
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IIndex<Type, IValidator> validators;

        public AutofacValidatorFactory(IIndex<Type, IValidator> validators)
        {
            this.validators = validators;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            IValidator validator;
            if (validators.TryGetValue(validatorType, out validator))
                return validator;
            return null;
        }
    }
}