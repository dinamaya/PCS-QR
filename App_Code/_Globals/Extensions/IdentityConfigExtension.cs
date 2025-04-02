using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Auth;
using System.Security.Claims;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class IdentityConfigExtension
	{
		public static void AddIdentityConfiguration(this IServiceCollection services)
		{
			services.AddIdentity<Account, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;
			})
				.AddEntityFrameworkStores<AuthDbContext>()
				.AddDefaultTokenProviders();

			services.AddScoped<PasswordHasher<Account>>();
		}
	}
}
