using CCIMS.Web.App_Code._Globals.Validtors.Objects;
using FluentValidation;
using FluentValidation.AspNetCore;


namespace CCIMS.Web.App_Code._Globals.Extensions
{
  public static class FluentValidationConfigExtension
  {
    public static void AddFluentValidationConfiguration(this IServiceCollection services)
    {
      services.AddFluentValidationAutoValidation();
      services.AddValidatorsFromAssemblyContaining<AccountCreationRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<AccountEditRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<ServicePartnerCreationRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<ServicePartnerEditRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<CaseUpdateRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<StatusCreationRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<StatusEditRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<CustomerEditRequestValidator>();
      services.AddValidatorsFromAssemblyContaining<CaseDetailsEditRequestValidator>();
    }
  }
}
