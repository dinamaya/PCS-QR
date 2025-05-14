using CCIMS.Web.App_Code._Globals.Validtors.Properties;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using FluentValidation;

namespace CCIMS.Web.App_Code._Globals.Validtors.Objects
{
  public class ServicePartnerEditRequestValidator : AbstractValidator<SPEditRequestDto>
  {
    public ServicePartnerEditRequestValidator(ServicePartnerCreationRequestValidator spCreationRequestValidator)
    {
      Include(spCreationRequestValidator);
    }
  }
}
