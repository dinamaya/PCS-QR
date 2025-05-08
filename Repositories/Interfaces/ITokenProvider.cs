using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ITokenProvider
	{
		string Generate(string qrHashedId);
		bool IsValid(string token, out QRTokenDto? qRToken);
    void Remove(string tokenKey);
	}
}
