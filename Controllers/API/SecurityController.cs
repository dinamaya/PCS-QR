using CCIMS.Web.Models.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
	[ApiController, Route("api/secure")]
	public class SecurityController : ControllerBase
	{
		private readonly ILogger<SecurityController> _logger;
		private readonly UserManager<Account> _userManager;

		public SecurityController(UserManager<Account> userManager, ILogger<SecurityController> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}

		// Provide password to hash
		[HttpGet("/hash/password")]
		public async Task<IActionResult> HashPassword(string username, string plainPassword)
		{
			try
			{
				var account = await _userManager.FindByNameAsync(username);

				if (account == null) return NotFound("Account not found");

				var hashedPassword = _userManager.PasswordHasher.HashPassword(account, plainPassword);

				return Ok(hashedPassword);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching companies.");
				return BadRequest(ex.Message);
			}
		}
	}
}
