using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using System.Collections.Concurrent;
using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;

namespace CCIMS.Web.Repositories.Implementations
{
	public class TokenProvider : ITokenProvider
	{
		private readonly ConcurrentDictionary<string, QRTokenDto> _validTokens = new();

		public string Generate(string qrHashedId)
		{
			string token = Utils.Security.GenerateExtendedGuid(string.Empty, 2);

			_validTokens[token] = new QRTokenDto
			{
				Expiry = DateTime.UtcNow.AddMinutes(Database.Token.EXPIRE_MIN),
				QRID = qrHashedId
      };

			return token;
		}

    public bool IsValid(string token, out QRTokenDto? qRToken)
		{
			if (string.IsNullOrWhiteSpace(token) || !_validTokens.TryGetValue(token, out var tokenInfo))
			{
				qRToken = null;
				return false;
			}
			
			if (DateTime.UtcNow > tokenInfo.Expiry)
			{
				_validTokens.TryRemove(token, out _);
				qRToken = null;
				return false;
			}

			qRToken = _validTokens.GetValueOrDefault(token)!;
			return qRToken != null;
		}

    public void Remove(string tokenKey) => _validTokens.TryRemove(tokenKey, out _);
  }
}
