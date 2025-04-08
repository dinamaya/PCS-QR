using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
	[Route("api/account")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly AuthDbContext _authDb;
		private readonly IAccountRepository _accountRepo;

		public AccountController(AuthDbContext authDb, IAccountRepository accountRepo)
		{
			_authDb = authDb;
			_accountRepo = accountRepo;
		}

		[HttpPost, Authorize]
		public async Task<ActionResult<ResponseDto>> Post([FromBody] AccountCreationRequestDto creationRequest)
		{
			var response = new ResponseDto();
			try
			{
				var date = DateTime.Now;
				string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
				await _accountRepo.CreateAsync(creationRequest, accountId);

				response.Message = "Account created successfully";

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.Message = "Error: " + ex.Message;
				response.IsSuccess = false;

				return BadRequest(response);
			}
		}
	}
}
