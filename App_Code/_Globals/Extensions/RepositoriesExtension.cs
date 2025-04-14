
using CCIMS.Web.Repositories;
using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Repositories.Interfaces.CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using CCIMS.Web.Services;

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
			services.AddScoped<IServicePartnerRepository, ServicePartnerRepository>();
			services.AddScoped<IAccountRepository, AccountRepository>();

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<ICaseRepository, CaseRepository>();
			services.AddScoped<ICustomerService, CustomerService>();

			services.AddSingleton<ITokenProvider, TokenProvider>();
		}
	}
}
