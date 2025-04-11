using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.Context;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class SQLConfigExtension
	{
		public static void AddSQLConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
		{
			services.AddDbContext<AuthDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbContext") ?? throw new Exception("Connection string AuthDbContext Not Found"));
			});

			services.AddDbContext<MainDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("MainDbContext") ?? throw new Exception("Connection string MainDbContext Not Found"));
			});

			services.AddDbContext<HangfireDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("HangfireDbContext") ?? throw new Exception("Connection string HangfireDbContext Not Found"));
			});
		}
	}
}
