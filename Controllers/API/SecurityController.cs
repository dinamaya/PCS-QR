using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
	[ApiController, Route("api/secure")]
	public class SecurityController : ControllerBase
	{
		private readonly ILogger<SecurityController> _logger;
		private readonly UserManager<Account> _userManager;
		private readonly ISecurityRepository _secureRepo;

    public SecurityController(UserManager<Account> userManager, ILogger<SecurityController> logger, ISecurityRepository secureRepo)
    {
      _userManager = userManager;
      _logger = logger;
      _secureRepo = secureRepo;
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

    // Provide password to hash
    [HttpGet("/qr/aes")]
    public async Task<IActionResult> HashQr(string qrId)
    {
      try
      {
        var id = await _secureRepo.EncryptIDAsync(qrId);

        return Ok(id);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while fetching companies.");
        return BadRequest(ex.Message);
      }
    }
  }
}
