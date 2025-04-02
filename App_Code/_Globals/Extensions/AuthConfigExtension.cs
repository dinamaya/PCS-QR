using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Factory;
using CCIMS.Web.Models.Entities.Auth;
using System.Security.Claims;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class AuthConfigExtension
	{
		public static void AddAuthConfiguration(this IServiceCollection services)
		{
			services.ConfigureApplicationCookie(options =>
			{
				options.AccessDeniedPath = "/Auth/RoleAccessDenied";
				options.Cookie.Name = Database.CURRENT_ACCOUNT;
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.Cookie.IsEssential = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(3);
				options.LoginPath = "/Auth/Login";
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
				options.SlidingExpiration = true;
			});

			services.AddHttpContextAccessor();
			services.AddHttpClient();
			services.AddScoped<IUserClaimsPrincipalFactory<Account>, AccountClaimsPrincipalFactory>();

			services.Configure<IdentityOptions>(options =>
			{
				options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
			});
		}
	}
}
