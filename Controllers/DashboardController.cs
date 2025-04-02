using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models;
using CCIMS.Web.Models.ViewModels;
using System.Diagnostics;

namespace CCIMS.Web.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{

		private readonly ILogger<DashboardController> _logger;

		public DashboardController(ILogger<DashboardController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

