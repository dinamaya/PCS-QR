
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.Models.Entities.Auth;


namespace CCIMS.Web.Context.Seeder
{
	public class AccountSeeder : Seeder
	{
    public override async Task Seed(IServiceProvider serviceProvider)
    {
      var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();
      using var context = new AuthDbContext(serviceProvider.GetRequiredService<DbContextOptions<AuthDbContext>>());

      var date = DateTime.Now;

      var p1 = new Person
      {
        FirstName = "Aldin",
        LastName = "Amaya",
        CreatedBy = "",
        ModifiedBy = "",
        DateCreated = date,
        DateModified = date,
        IsActive = true
      };

      var p2 = new Person
      {
        FirstName = "Ken",
        LastName = "Napura",
        CreatedBy = "",
        ModifiedBy = "",
        DateCreated = date,
        DateModified = date,
        IsActive = true
      };

      if (!context.People.Any())
      {
        await context.People.AddRangeAsync(p1, p2);
        await context.SaveChangesAsync();

        if (!userManager.GetUsersInRoleAsync("Admin").GetAwaiter().GetResult().Any())
        {
          var acc1 = new Account
          {
            UserName = "aldin_amaya",
            PersonID = context.People.FindAsync(p1.Id).Result.Id,
            Email = "aldin_amaya@gmail.com",
            CreatedBy = "1",
            ModifiedBy = "1",
            DateCreated = date,
            DateModified = date,
            IsActive = true
          };

          acc1.CreatedBy = acc1.Id;
          acc1.ModifiedBy = acc1.Id;

          var acc2 = new Account
          {
            UserName = "kenjie_sama",
            PersonID = context.People.FindAsync(p2.Id).Result.Id,
            Email = "kenjie@gmail.com",
            CreatedBy = acc1.Id,
            ModifiedBy = acc1.Id,
            DateCreated = date,
            DateModified = date,
            IsActive = true
          };

          if (!context.Accounts.Any())
          {
            var result1 = await userManager.CreateAsync(acc1, "Super_Admin!22");
            if (!result1.Succeeded)
              throw new Exception($"Failed to create user {acc1.UserName}: {string.Join(", ", result1.Errors.Select(e => e.Description))}");

            var result2 = await userManager.CreateAsync(acc2, "Kenjie4Ever!");
            if (!result2.Succeeded)
              throw new Exception($"Failed to create user {acc2.UserName}: {string.Join(", ", result2.Errors.Select(e => e.Description))}");

            await context.SaveChangesAsync();

            context.Accounts.Update(acc1);
            await context.SaveChangesAsync();
          }

          await userManager.AddToRoleAsync(acc1, "OPS");
          await userManager.AddToRoleAsync(acc2, "SPA");
          await context.SaveChangesAsync();
        }
      }

    }

    public static async Task Run(IServiceProvider serviceProvider) => await new AccountSeeder().Seed(serviceProvider);
  }
}
