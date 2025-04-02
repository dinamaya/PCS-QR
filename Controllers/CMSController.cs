using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers
{
	public class CMSController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> QR(string? v = null)
		{
			
			return View();
		}
	}
}
