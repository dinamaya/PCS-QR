using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.DTOs;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class CustomerEditRequestValidator : AbstractValidator<CustomerEditRequestDto>
  {
    public CustomerEditRequestValidator() {
      RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id Should not be empty");

      RuleFor(x => x.FirstName)
        .NotEmpty().WithMessage("First name is required.")
        .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
        .Matches(RegEx.NAMES).WithMessage("First name contains invalid characters.");

      RuleFor(x => x.LastName)
          .NotEmpty().WithMessage("Last name is required.")
          .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
          .Matches(RegEx.NAMES).WithMessage("Last name contains invalid characters.");

      RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email must be valid.")
        .Matches(RegEx.EMAIL_STRICT).WithMessage("Email contains invalid characters.");
      
      RuleFor(x => x.ContactNo)
        .NotEmpty().WithMessage("Contact Number is required.")
        .Matches(RegEx.CONTACT_NUMBER).WithMessage("Contact Number contains invalid characters.");

      RuleFor(x => x.Address)
        .NotEmpty().WithMessage("Address is required.");

    }
  }
}
