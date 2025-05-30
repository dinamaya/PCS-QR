using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Services.Implementations;
using CCIMS.Web.Services.Interfaces;
using Hangfire;
using System.Diagnostics;

namespace CCIMS.Web.App_Code._Globals
{
  public class BackgroundJobsInitializer
  {
    public static void Run()
    {
      RecurringJob.AddOrUpdate<IBackgroundJobsService>(
        "minutely-aging-job",
        service => service.ExecuteAsync(),
        Cron.Minutely
      );

      RecurringJob.AddOrUpdate(
        "minutely-job",
        () => Debug.WriteLine("Successfully Executed Minutely Job"),
        Cron.Minutely
      );

      RecurringJob.AddOrUpdate<IBackgroundJobsService>(
        "test-minutely-job",
        service => service.TestExecuteAsync(),
        Cron.Minutely
      );
    }
  }
}
