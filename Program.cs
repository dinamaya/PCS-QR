using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Context.Seeder;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Implementations;
using CCIMS.Web.Services.Interfaces;
using Hangfire;
using System.Diagnostics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


#region Extension Configurations
builder.Services.AddIdentityConfiguration();
builder.Services.AddAuthConfiguration();
builder.Services.AddSQLConfiguration(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddComplexConfiguration(builder.Configuration);
builder.Services.AddFluentValidationConfiguration();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHangfireConfigExtension();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard(builder.Configuration.GetSection("Hangfire:dashboard").Get<string>(), new DashboardOptions()
{
  DashboardTitle = "CCI Jobs Monitoring"
});
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");


#region Database Seeding
using (var scope = app.Services.CreateScope())
{
  //Run Only in Dev Mode
  // Disable this on production to avoid errors
  // Populate the Database using the Any Development Project.
  if (app.Environment.IsDevelopment())
  {
    var services = scope.ServiceProvider;
    await AccountSeeder.Run(services);
    await ServicePartnerSeeder.Run(services);
    await StatusSeeder.Run(services);
  }
}
#endregion

#region Hangfire Job Initialization
using (var scope = app.Services.CreateScope())
{
  var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
  var timeList = configuration.GetSection("Hangfire:time").Get<IEnumerable<string>>();
  var timezone = configuration.GetSection("Hangfire:timezone").Get<string>();
  
  foreach (var timeString in timeList)
  {
    if (TimeSpan.TryParseExact(timeString, new[] { @"h\:mm", @"hh\:mm" }, CultureInfo.InvariantCulture, out var time))
    {
      string cron = Cron.Daily(time.Hours, time.Minutes);
      string jobId = $"send-aged-cases-at-{time:hh\\:mm}";

      RecurringJob.AddOrUpdate<BackgroundJobsService>(
          jobId,
          service => service.SendCasesAgedEmail(),
          cron,
          TimeZoneInfo.FindSystemTimeZoneById(timezone)
      );
    }
    else
    {
      Console.WriteLine($"Invalid time format in Hangfire:time config: {timeString}");
    }
  }
}
#endregion

app.Run();
