using System;
using ASTRA.EMSG.Business.Infrastructure.Validators;
using ASTRA.EMSG.Business.Models.Katalogs;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using FluentValidation;

namespace ASTRA.EMSG.Business.Validators.Katalogs
{
    public class MassnahmenvorschlagKatalogCreateModelValidator : AbstractValidatorBase<MassnahmenvorschlagKatalogCreateModel>
    {
        public MassnahmenvorschlagKatalogCreateModelValidator(ILocalizationService localizationService, IGlobalMassnahmenvorschlagKatalogEditService globalMassnahmenvorschlagKatalogEditService)
            : base(localizationService)
        {
            RuleForNotNullableString(m => m.Typ);
            RuleFor(m => m.KonstenModels).SetCollectionValidator(new MassnahmenvorschlagKatalogKonstenEditModelValidator(localizationService));
            RuleFor(c => c.Typ).Must(globalMassnahmenvorschlagKatalogEditService.IsTypUniqe).When(m => m.Id == Guid.Empty).WithMessage(localizationService.GetLocalizedError(ValidationError.ShouldBeUnique));
        }
    }

    public class MassnahmenvorschlagKatalogEditModeValidator : AbstractValidatorBase<MassnahmenvorschlagKatalogEditModel>
    {
        public MassnahmenvorschlagKatalogEditModeValidator(ILocalizationService localizationService)
            : base(localizationService)
        {
            RuleForNotNullableString(m => m.Typ);
            RuleFor(m => m.KonstenModels).SetCollectionValidator(new MassnahmenvorschlagKatalogKonstenEditModelValidator(localizationService));
        }
    }
}