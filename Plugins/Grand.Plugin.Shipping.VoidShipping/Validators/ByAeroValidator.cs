using FluentValidation;
using Grand.Services.Localization;
using Grand.Web.Framework.Validators;
using Grand.Plugin.Shipping.VoidShipping.Models;

namespace Grand.Plugin.Shipping.VoidShipping.Validators
{
    class ByAeroValidator : BaseNopValidator<ByAeroModel>
    {
        public ByAeroValidator(ILocalizationService localizationService)
        {
            //useful links:
            //http://fluentvalidation.codeplex.com/wikipage?title=Custom&referringTitle=Documentation&ANCHOR#CustomValidator
            //http://benjii.me/2010/11/credit-card-validator-attribute-for-asp-net-mvc-3/

            RuleFor(x => x.Address).NotEmpty().WithMessage("Address shouldnt be empty");
            RuleFor(x => x.Address).NotEqual("no address").WithMessage("Please write valid address");
            RuleFor(x => x.SomeNumber).NotEmpty().WithMessage("Number shouldnt be empty");
            RuleFor(x => x.SomeNumber).GreaterThan(0).WithMessage("Number should be greater than 0");
        }
    }
}