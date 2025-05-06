using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Validtors.Properties;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using FluentValidation;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class AccountEditRequestValidator : AbstractValidator<AccountEditRequestDto>
  {
  
    public AccountEditRequestValidator(AccountBasicInfoValidator accountBasicInfoValidator)
    {
      Include(accountBasicInfoValidator);

      RuleFor(x => x.Password)
        .Equal(x => x.RetypePass).WithMessage("Passwords do not match.")
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
        .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
        .When(x =>
        {
          bool res = !string.IsNullOrEmpty(x.PasswordResetType) && x.PasswordResetType.Equals("Change");
          return res;
        });

      RuleFor(x => x.RetypePass)
        .Equal(x => x.Password).WithMessage("Passwords do not match.")
        .NotEmpty().WithMessage("Retype Password is required.")
        .When(x =>
        {
          bool res = !string.IsNullOrEmpty(x.PasswordResetType) && x.PasswordResetType.Equals("Change");
          return res;
        });

    }
  }
}
