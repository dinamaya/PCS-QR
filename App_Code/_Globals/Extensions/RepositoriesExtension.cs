
using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.App_Code._Globals.Extensions
{
	public static class RepositoriesExtension
	{
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IAuthRepository, AuthRepository>();
			services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
			services.AddScoped<ISecurityRepository, SecurityRepository>();
			services.AddScoped<IQRRepository, QRRepository>();

			services.AddSingleton<ITokenProvider, TokenProvider>();
		}
	}
}
