using Microsoft.AspNetCore.Identity;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Repositories.Interfaces;
using System.Security.Claims;

namespace CCIMS.Web.Repositories.Implementations
{
	public class AuthRepository : IAuthRepository
	{
		private readonly AuthDbContext _authDb;
		private readonly UserManager<Account> _userManager;
		private readonly SignInManager<Account> _signInManager;

		public AuthRepository(AuthDbContext authDb, UserManager<Account> userManager, SignInManager<Account> signInManager)
		{
			_authDb = authDb;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public bool IsSignedIn(ClaimsPrincipal authClaims) => _signInManager.IsSignedIn(authClaims);

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			try
			{
				var signInResult = await _signInManager
				.PasswordSignInAsync(
					loginRequestDto.Username,
					loginRequestDto.PlaintextPassword,
					loginRequestDto.IsRemembered,
					false
				);

				if (!signInResult.Succeeded)
					throw new Exception(Exceptions.Message.INVALID_AUTHENTICATION_CREDENTIALS);

				var user = await _userManager.FindByNameAsync(loginRequestDto.Username) ?? throw new Exception(Exceptions.Message.INVALID_AUTHENTICATION_CREDENTIALS);

				return new()
				{
					Result = "Success",
					Message = null
				};
			}
			catch (Exception ex) 
			{
				return new LoginResponseDto()
				{
					Result = null,
					Message = ex.Message
				};
			}
		}

		public async Task Logout(ClaimsPrincipal authClaims)
		{
			var _account = authClaims.Identities.FirstOrDefault().IsAuthenticated;
			if (_account)
				await _signInManager.SignOutAsync();

		}
	}
}
