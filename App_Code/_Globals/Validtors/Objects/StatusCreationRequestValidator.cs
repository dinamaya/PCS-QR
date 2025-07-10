using CCIMS.Web.Models.DTOs;
using FluentValidation;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class StatusCreationRequestValidator : AbstractValidator<StatusDto>
  {
    public StatusCreationRequestValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Status name is Required")
        .NotNull().WithMessage("Status name is Required");
    }
  }
}
