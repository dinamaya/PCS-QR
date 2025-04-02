using Microsoft.AspNetCore.Identity;
using CCIMS.Web.Models.Entities.Auth;

namespace CCIMS.Web.Context.Seeder
{
	public abstract class Seeder
	{
		public abstract Task Seed(IServiceProvider serviceProvider);
		public abstract Task Seed(IServiceProvider serviceProvider, UserManager<Account> userManager);
	}
}
