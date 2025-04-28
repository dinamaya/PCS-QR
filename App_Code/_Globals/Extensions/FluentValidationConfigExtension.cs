using CCIMS.Web.App_Code._Globals.Factory;
using CCIMS.Web.App_Code._Globals.Validtors.Objects;
using CCIMS.Web.App_Code._Globals.Validtors.Properties;
using CCIMS.Web.Models.DTOs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
  public static class FluentValidationConfigExtension
  {
    public static void AddFluentValidationConfiguration(this IServiceCollection services)
    {
      services.AddFluentValidationAutoValidation();
      services.AddValidatorsFromAssemblyContaining<AccountCreationRequestValidator>();
    }
  }
}
