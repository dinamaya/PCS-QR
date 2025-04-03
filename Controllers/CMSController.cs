using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.Controllers
{
	public class CMSController : Controller
	{
    private readonly IServicePartnerRepository _spRepo;

    public CMSController(IServicePartnerRepository spRepo)
    {
      _spRepo = spRepo;
    }
    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> ServicePartner()
    {
      try
      {
        var allSp = await _spRepo.GetAll();
        return View(allSp);
      }
      catch (Exception ex)
      {
        return View(Enumerable.Empty<ServicePartnerRowViewModel>());
      }
    }

  }
}
