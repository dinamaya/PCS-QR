namespace CCIMS.Web.Services.Interfaces
{
  public interface IBackgroundJobsService
  {
    Task ExecuteAsync();
    Task TestExecuteAsync();
  }
}
