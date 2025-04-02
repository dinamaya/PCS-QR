using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ITokenProvider
	{
		string GenerateToken(int minutesToExpire, int remainingUses, string qrHashedId);
		bool IsValidToken(string token, out QRTokenDto? qRToken);
		//QRTokenDto GetToken(string key);
	}
}
