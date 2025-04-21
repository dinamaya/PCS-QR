using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Models.Entities.Main;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CCIMS.Web.Context.Seeder
{
  public class StatusSeeder : Seeder
  {
    public override async Task Seed(IServiceProvider serviceProvider)
    {
      var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();
      using var context = new MainDbContext(serviceProvider.GetRequiredService<DbContextOptions<MainDbContext>>());
      using var auth = new AuthDbContext(serviceProvider.GetRequiredService<DbContextOptions<AuthDbContext>>());
      
      var date = DateTime.Now;

      var sp1 = await userManager.FindByNameAsync("kenjie_sama") ?? throw new Exception();

      if (context.Statuses.Any(s => s.Name == "On-Queue"))
        return;

      var st1 = Create("On-Queue", false, sp1.Id);
      var st2 = Create("For DTS Approval", true, sp1.Id);
      var st3 = Create("Parts Requested", true, sp1.Id);
      var st4 = Create("Closed", true, sp1.Id);

      await context.Statuses.AddRangeAsync(st1, st2, st3, st4);
      await context.SaveChangesAsync();
    }

    private Status Create(string name, bool isCommentable, string createdBy)
    {
      return new Status()
      {
        Name = name,
        IsCommentable = isCommentable,
        CreatedBy = createdBy,
        DateCreated = DateTime.Now.ToLocalTime(),
        ModifiedBy = createdBy,
        DateModified = DateTime.Now.ToLocalTime(),
        IsActive = true,
      };
    }

    public static async Task Run(IServiceProvider serviceProvider) => await new StatusSeeder().Seed(serviceProvider);
  }
}
