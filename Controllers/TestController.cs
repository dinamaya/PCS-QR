using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers
{
  public class TestController : Controller
  {
    public IActionResult CaseEmail()
    {
      return View();
    }
  }
}
