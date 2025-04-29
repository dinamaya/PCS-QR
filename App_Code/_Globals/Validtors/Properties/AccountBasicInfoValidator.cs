using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.App_Code._Globals.Validtors.Properties
{
  public class AccountBasicInfoValidator : AbstractValidator<AccountBasicInfoDto>
  {
    public AccountBasicInfoValidator(IConfigurationRepository configRepo)
    {
      RuleFor(x => x.FirstName)
        .NotEmpty().WithMessage("First name is required.")
        .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
        .Matches(RegEx.NAMES).WithMessage("First name contains invalid characters.");

      RuleFor(x => x.LastName)
          .NotEmpty().WithMessage("Last name is required.")
          .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
          .Matches(RegEx.NAMES).WithMessage("Last name contains invalid characters.");

      RuleFor(x => x.Username)
        .NotEmpty().WithMessage("Username is required.")
        .MinimumLength(4)
          .WithMessage("Username must be at least 4 characters.")
        .MaximumLength(30)
          .WithMessage("Username must not exceed 30 characters.")
        .Must(username => !string.IsNullOrEmpty(username) && !username.StartsWith("_") && !username.StartsWith("."))
          .WithMessage("Username cannot start with an underscore or dot")
        .Must(username => !string.IsNullOrEmpty(username) && !username.EndsWith("_") && !username.EndsWith("."))
          .WithMessage("Username cannot end with an underscore or dot")
        .Must(username => !string.IsNullOrEmpty(username) && !username.Contains("__") && !username.Contains("..") && !username.Contains("._") && !username.Contains("_."))
          .WithMessage("Username cannot contain consecutive underscores or dots (e.g., '__', '..', '._', '_.')")
        .Matches(RegEx.USERNAME)
          .WithMessage("Username contains invalid characters. only letters, numbers, underscores, and dots are ");

      RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email must be valid.")
        .Matches(RegEx.EMAIL_LOCAL).WithMessage("Email username part contains invalid characters.")
        .Must(email =>
        {
          var allowedDomains = configRepo.GetAllowedEmailDomains();

          if (email.IsNullOrEmpty())
            return false;

          var domainPart = email.Split('@').LastOrDefault()?.Split('.').FirstOrDefault()?.ToLowerInvariant();

          return !string.IsNullOrWhiteSpace(domainPart) && allowedDomains.Contains(domainPart);
        }).WithMessage("Email domain is not allowed.");

      RuleFor(x => x.AccountType)
        .NotEmpty().WithMessage("Account type is required.")
        .Must(type => type == "SPA" || type == "OPS")
        .WithMessage("Account type must be either \"SPA\" or \"OPS\".")
        .MinimumLength(3).WithMessage("Account type must be at least 4 characters.")
        .MaximumLength(3).WithMessage("Account type must not exceed 30 characters.")
        .Matches(RegEx.NAMES).WithMessage("Account type contains invalid characters.");

    }
  }
}
