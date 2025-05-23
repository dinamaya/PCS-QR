using CCIMS.Web.App_Code._Globals.Factory;
using CCIMS.Web.Models.Complex;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
  public static class ComplexConfigExtension
  {
    public static void AddComplexConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<EmailServiceConfig>(configuration.GetSection("EmailServiceConfig"));
    }
  }
}
