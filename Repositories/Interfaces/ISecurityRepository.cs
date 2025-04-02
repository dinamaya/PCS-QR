namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ISecurityRepository
  {
    Task<string> EncryptAccountIDAsync(string accountId);
    Task<string> DecryptAccountIDAsync(string encryptedAccountId);
    Task<string> EncryptIDAsync(string id);
    Task<string> DecryptIDAsync(string encryptedId);
	}
}
