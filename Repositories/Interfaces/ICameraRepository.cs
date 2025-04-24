namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ICameraRepository
  {
    Task<string> ExtractText(string filePath);
  }
}
