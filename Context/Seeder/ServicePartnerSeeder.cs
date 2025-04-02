using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Models.Entities.Main;

namespace CCIMS.Web.Context.Seeder
{
	public class ServicePartnerSeeder : Seeder
	{
		public override async Task Seed(IServiceProvider serviceProvider, UserManager<Account> userManager)
		{
			using var context = new MainDbContext(serviceProvider.GetRequiredService<DbContextOptions<MainDbContext>>());

			var date = DateTime.Now;

			if (!context.ServicePartners.Any())
			{
				ServicePartner sp1 = new ServicePartner
				{
					Name = "Tech Solutions Inc.",
					ContactNumber = "123-456-7890",
					Email = "contact@techsolutions.com",
					CompanyName = "Microsoft",
          ContactPerson = "John Doe",
					CreatedBy = "",
					DateCreated = date,
					ModifiedBy = "",
					DateModified = date,
					IsActive = true
				};

				ServicePartner sp2 = new ServicePartner
				{
					Name = "Auto Parts Plus",
					ContactNumber = "987-654-3210",
					Email = "info@autopartsplus.com",
					CompanyName = "Pure Gold",
					ContactPerson = "Jane Smith",
					CreatedBy = "",
					DateCreated = date,
					ModifiedBy = "",
					DateModified = date,
					IsActive = true
				};

				ServicePartner sp3 = new ServicePartner
				{
					Name = "Rapid Repair Services",
					ContactNumber = "555-123-4567",
					Email = "support@rapidrepair.com",
					CompanyName = "SM",
					ContactPerson = "Mike Johnson",
					CreatedBy = "",
					DateCreated = date,
					ModifiedBy = "",
					DateModified = date,
					IsActive = true
				};

        await context.ServicePartners.AddRangeAsync(sp1, sp2, sp3);
				await context.SaveChangesAsync();
			}
		}

		public static async Task Run(IServiceProvider serviceProvider, UserManager<Account> userManager) => await new ServicePartnerSeeder().Seed(serviceProvider, userManager);

		public override async Task Seed(IServiceProvider serviceProvider)
		{
			throw new NotImplementedException();
		}
	}
}