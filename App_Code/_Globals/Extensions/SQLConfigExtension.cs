using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.Context;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class SQLConfigExtension
	{
		public static void AddSQLConfiguration(this IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddDbContext<AuthDbContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("AuthDbContext") ?? throw new Exception("Connection string AuthDbContext Not Found"));
			});

			services.AddDbContext<MainDbContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("MainDbContext") ?? throw new Exception("Connection string MainDbContext Not Found"));
			});

			services.AddDbContext<HangfireDbContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("HangfireDbContext") ?? throw new Exception("Connection string HangfireDbContext Not Found"));
			});
		}
	}
}
