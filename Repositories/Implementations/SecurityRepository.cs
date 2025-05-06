using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CCIMS.Web.Repositories.Implementations
{
  public class SecurityRepository : ISecurityRepository
  {
    private readonly UserManager<Account> _userManager;
    private readonly IConfigurationRepository _configService;
    private readonly ILogger<SecurityRepository> _logger;

    public SecurityRepository(UserManager<Account> userManager, IConfigurationRepository configService, ILogger<SecurityRepository> logger)
    {
      _userManager = userManager;
      _configService = configService;
      _logger = logger;
    }

    public async Task<string> EncryptAccountIDAsync(string accountId)
    {
      var sysAdminDetails = _configService.GetSysAdminPrivateDetails();

      try
      {
        using Aes aes = Aes.Create();
        aes.Key = sysAdminDetails.PrivateKey;
        aes.IV = sysAdminDetails.PrivateIV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
          byte[] plainBytes = Encoding.UTF8.GetBytes(accountId);
          await cryptoStream.WriteAsync(plainBytes, 0, plainBytes.Length);
          await cryptoStream.FlushFinalBlockAsync();
        }
        return Convert.ToBase64String(memoryStream.ToArray());
      }
      catch (Exception ex)
      {
        _logger.LogError($"Encryption failed: {ex.Message}");
        throw new Exception("Encryption failed", ex);
      }
    }

    public async Task<string> DecryptAccountIDAsync(string encryptedAccountId)
    {
      var sysAdminDetails = _configService.GetSysAdminPrivateDetails();

      try
      {
        using Aes aes = Aes.Create();
        aes.Key = sysAdminDetails.PrivateKey;
        aes.IV = sysAdminDetails.PrivateIV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedAccountId));
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
        string decryptedString = await reader.ReadToEndAsync();
        return decryptedString;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Decryption failed: {ex.Message}");
        throw new Exception("Decryption failed", ex);
      }
    }

		public async Task<string> EncryptIDAsync(string id)
		{
			var sysAdminDetails = _configService.GetSysAdminPrivateDetails();

			try
			{
				using Aes aes = Aes.Create();
				aes.Key = sysAdminDetails.PrivateKey;
				aes.IV = sysAdminDetails.PrivateIV;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
				using var memoryStream = new MemoryStream();
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					byte[] plainBytes = Encoding.UTF8.GetBytes(id);
					await cryptoStream.WriteAsync(plainBytes, 0, plainBytes.Length);
					await cryptoStream.FlushFinalBlockAsync();
				}

				byte[] encryptedBytes = memoryStream.ToArray();

				StringBuilder hexString = new StringBuilder(encryptedBytes.Length * 2);
				foreach (byte b in encryptedBytes)
					hexString.Append(b.ToString("x2"));

				return hexString.ToString();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Encryption failed: {ex.Message}");
				throw new Exception("Encryption failed", ex);
			}
		}

		public async Task<string> DecryptIDAsync(string encryptedId)
		{
			var sysAdminDetails = _configService.GetSysAdminPrivateDetails();

			try
			{
				using Aes aes = Aes.Create();
				aes.Key = sysAdminDetails.PrivateKey;
				aes.IV = sysAdminDetails.PrivateIV;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				byte[] encryptedBytes = Utils.Security.ConvertHexStringToBytes(encryptedId);

				using var memoryStream = new MemoryStream(encryptedBytes);
				using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

				using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
				string decryptedString = await reader.ReadToEndAsync();
				return decryptedString;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Decryption failed: {ex.Message}");
				throw new Exception("Decryption failed", ex);
			}
		}
	}
}
