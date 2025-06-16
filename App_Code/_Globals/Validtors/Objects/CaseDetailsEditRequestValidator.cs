using CCIMS.Web.Models.DTOs;
using FluentValidation;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class CaseDetailsEditRequestValidator : AbstractValidator<CaseEditRequestDto>
  {
    public CaseDetailsEditRequestValidator()
    {
      RuleFor(x => x.ServicePartner)
         .NotEmpty().WithMessage("Service Partner is Required")
         .NotNull().WithMessage("Service Partner is Required");  

      RuleFor(x => x.SerialNumber)
         .NotEmpty().WithMessage("Serial Number is Required")
         .NotNull().WithMessage("Serial Number is Required");  
    }
  }
}
