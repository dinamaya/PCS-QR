using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
