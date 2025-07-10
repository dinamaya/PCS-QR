using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.DTOs;
using FluentValidation;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class TransactionEditRequestValidator : AbstractValidator<TransactionEditRequestDto>
  {
    public TransactionEditRequestValidator()
    {
      RuleFor(x => x.Remarks)
        .NotEmpty().WithMessage("Transaction Remarks is required.")
        .MaximumLength(3000).WithMessage("Transaction Remarks must not exceed 3000 characters.");
    }
  }
}
