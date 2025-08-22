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
				options.Cookie.SameSite = SameSiteMode.None;
				options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
                options.SlidingExpiration = true;

                options.LoginPath = "/Auth/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;

                options.Events.OnSigningIn = context =>
                {
                    var props = context.Properties;

                    if (props.IsPersistent)
                    {
                        props.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5);
                    }
                    else 
                    {
                        props.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(3);
                        props.IsPersistent = false;
                    }

                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
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
