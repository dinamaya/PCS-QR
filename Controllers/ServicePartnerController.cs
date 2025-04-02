using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Controllers
{
	public class ServicePartnerController : Controller
	{
		public async Task<IActionResult> ServicePartner(string v)
		{
			if(v.IsNullOrEmpty())
				return View(Enumerable.Empty<ServicePartnerRowViewModel>());
			

			return View(Enumerable.Empty<ServicePartnerRowViewModel>());
		}
	}
}
