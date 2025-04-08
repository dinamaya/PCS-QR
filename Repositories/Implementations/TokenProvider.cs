using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using System.Collections.Concurrent;
using CCIMS.Web.App_Code._Globals;

namespace CCIMS.Web.Repositories.Implementations
{
	public class TokenProvider : ITokenProvider
	{
		private readonly ConcurrentDictionary<string, QRTokenDto> _validTokens = new();

		public string GenerateToken(int minutesToExpire, int remainingUses, string qrHashedId)
		{
			string token = Utils.Security.GenerateExtendedGuid("tk",5);

			_validTokens[token] = new QRTokenDto
			{
				Expiry = DateTime.UtcNow.AddMinutes(minutesToExpire),
				RemainingUses = remainingUses,
				QRID = qrHashedId
      };

			return token;
		}

    public bool IsValidToken(string token, out QRTokenDto? qRToken)
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

			if (--tokenInfo.RemainingUses <= 0)
				_validTokens.TryRemove(token, out _);

			qRToken = _validTokens.GetValueOrDefault(token)!;
			return qRToken != null;
		}
	}
}
