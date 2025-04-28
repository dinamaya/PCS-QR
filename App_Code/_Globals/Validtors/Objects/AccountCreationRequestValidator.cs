using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Validtors.Properties;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Repositories.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class AccountCreationRequestValidator : AbstractValidator<AccountCreationRequestDto>
  {
    public AccountCreationRequestValidator(AccountBasicInfoValidator accountBasicInfoValidator)
    {
      Include(accountBasicInfoValidator);

      RuleFor(x => x.Password)
          .Equal(x => x.RetypePass).WithMessage("Passwords are not equals")
          .NotEmpty().WithMessage("Password is required.")
          .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
          .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

      RuleFor(x => x.RetypePass)
        .Equal(x => x.Password).WithMessage("Passwords are not equals")
        .NotEmpty().WithMessage("Retype Password is required.");
    }
  }
}
