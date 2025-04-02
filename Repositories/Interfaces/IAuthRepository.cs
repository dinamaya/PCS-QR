using CCIMS.Web.Models.DTOs;
using System.Security.Claims;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IAuthRepository
	{
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
		Task Logout(ClaimsPrincipal authClaims);
		bool IsSignedIn(ClaimsPrincipal authClaims);
	}
}
