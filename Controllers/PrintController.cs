using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers
{
  public class PrintController : Controller
  {
    public IActionResult QR(string id)
    {
      return View();
    }
  }
}
