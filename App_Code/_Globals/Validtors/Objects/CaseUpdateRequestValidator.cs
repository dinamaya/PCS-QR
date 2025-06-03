using CCIMS.Web.Models.DTOs;
using FluentValidation;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class CaseUpdateRequestValidator : AbstractValidator<CaseStatusUpdateRequestDto>
  {
    public CaseUpdateRequestValidator()
    {
      RuleFor(x => x.StatusId)
       .NotEmpty().WithMessage("Status is Required");
    }
  }
}
