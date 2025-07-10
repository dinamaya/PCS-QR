using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class ServicePartnerCreationRequestValidator : AbstractValidator<SPCreationRequestDto>
  {
    public ServicePartnerCreationRequestValidator(IConfigurationRepository configRepo)
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Service Partner name is required.")
        .MaximumLength(200).WithMessage("Service Partner name must not exceed 200 characters.")
        .Matches(RegEx.COMMON).WithMessage("Service Partner name contains invalid characters.");

      RuleFor(x => x.CompanyName)
        .NotEmpty().WithMessage("Company name is required.")
        .MaximumLength(200).WithMessage("Company name must not exceed 200 characters.")
        .Matches(RegEx.COMMON).WithMessage("Company name contains invalid characters.");

      RuleFor(x => x.ContactNumber)
        .NotEmpty().WithMessage("Contact Number is required.")
        .MaximumLength(20).WithMessage("Contact Number must not exceed 20 characters.")
        .Matches(RegEx.CONTACT_NUMBER).WithMessage("Contact Number contains invalid characters.");

      RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email must be valid.")
        .Matches(RegEx.EMAIL_STRICT).WithMessage("Email contains invalid characters.");

      RuleFor(x => x.ContactPerson)
        .NotEmpty().WithMessage("Contact Person is required.")
        .MaximumLength(200).WithMessage("Contact Person must not exceed 200 characters.")
        .Matches(RegEx.NAMES).WithMessage("Contact Person contains invalid characters.");

    }
  }
}
