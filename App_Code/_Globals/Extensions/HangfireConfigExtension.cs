using CCIMS.Web.App_Code._Globals.Factory;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Hangfire;
using static CCIMS.Web.App_Code._Globals.Constants.Routes.Partials;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class HangfireConfigExtension
	{
		public static void AddHangfireConfigExtension(this IServiceCollection services)
		{
			services.AddHangfire((provider, option) => {
				var connString = provider.GetRequiredService<IConfiguration>().GetConnectionString("HangfireDbContext");
				option.UseSqlServerStorage(connString);
			});

			services.AddHangfireServer();
		}
	}
}
