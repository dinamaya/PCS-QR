using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Repositories.Interfaces;
namespace CCIMS.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly ILogger<AuthController> _logger;
		private readonly IAuthRepository _authRepo;

		public AuthController(ILogger<AuthController> logger, IAuthRepository authRepo)
		{
			_logger = logger;
			_authRepo = authRepo;
		}

		public IActionResult Index()
		{
			return RedirectToAction("Login");
		}

		[HttpGet]
		public IActionResult Login(string q = "")
		{
			try
			{
				if (q.Equals(Queries.SIGNOUT))
					ViewData[Keys.ViewData.SIGN_OUT] = "You have been logged out.";

				return _authRepo.IsSignedIn(User) ? RedirectToAction("Index", "Home") : View();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());

				return RedirectToAction("Index", "Auth");
			}
		}


		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDto loginRequestDTO, string returnUrl = null)
		{
			try
			{
				var loginResponseDTO = await _authRepo.Login(loginRequestDTO);

				if (loginResponseDTO.Result == null)
				{
					// Invalid Login
					ViewData[Keys.ViewData.ERROR] = loginResponseDTO.Message;
					return View(loginRequestDTO);
				}

				// Login Account					
				if (loginResponseDTO.Result != null)
					return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");

				return View(loginRequestDTO);
			}
			catch (Exception ex)
			{
				ViewData[Keys.ViewData.ERROR] = ex.Message + " " + ex.InnerException?.Message;
				return RedirectToAction("Index", "Auth");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _authRepo.Logout(User);
			return RedirectToAction("Index", "Auth");
		}

	}
}