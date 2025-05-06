using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Context.Seeder;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


#region Extension Configurations
builder.Services.AddIdentityConfiguration();
builder.Services.AddAuthConfiguration();
builder.Services.AddHangfireConfigExtension();
builder.Services.AddSQLConfiguration(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddFluentValidationConfiguration();

#endregion


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard(builder.Configuration.GetSection("Hangfire").Get<string>(), new DashboardOptions()
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

app.Run();
