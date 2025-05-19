
using CCIMS.Web.Repositories;
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
			services.AddScoped<IServicePartnerRepository, ServicePartnerRepository>();
			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<IOperationsRepository, OperationsRepository>();

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<ICaseRepository, CaseRepository>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<ICameraRepository, CameraRepository>();
			services.AddScoped<IDashboardRepository, DashboardRepository>();

			services.AddSingleton<ITokenProvider, TokenProvider>();

      services.AddSingleton<FileManager>();
      services.AddSingleton<Server>();
    }
  }
}
