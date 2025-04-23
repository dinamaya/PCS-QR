using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models;
using CCIMS.Web.Models.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using CCIMS.Web.Models.Entities.Auth;

namespace CCIMS.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SignInManager<Account> _signInManager;

		public HomeController(ILogger<HomeController> logger, SignInManager<Account> signInManager)
		{
			_logger = logger;
			_signInManager = signInManager;
		}

		public IActionResult Index() => _signInManager.IsSignedIn(User) ? RedirectToAction("Index", "Dashboard") : RedirectToAction("Index", "Auth");

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
