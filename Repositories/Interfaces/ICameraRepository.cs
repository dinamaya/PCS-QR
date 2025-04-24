namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ICameraRepository
  {
    Task<string> GetResult(string filePath);
  }
}
