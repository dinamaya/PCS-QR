using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.Controllers
{
  [Authorize]
	public class CMSController : Controller
	{
    private readonly IServicePartnerRepository _spRepo;
    private readonly IAccountRepository _accountRepo;

		public CMSController(IServicePartnerRepository spRepo, IAccountRepository accountRepo)
		{
			_spRepo = spRepo;
			_accountRepo = accountRepo;
		}
		public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> ServicePartner()
    {
      try
      {
        var results = await _spRepo.GetAll();
        return View(results);
      }
      catch (Exception ex)
      {
        return View(Enumerable.Empty<ServicePartnerRowViewModel>());
      }
    }

    public async Task<IActionResult> Account()
    {
      try
      {
        var results = await _accountRepo.GetAll();
        ViewData[Keys.ViewData.Types.ROLES] = await _accountRepo.GetAllRoles();

				return View(results);
      }
      catch (Exception ex)
      {
        return View(Enumerable.Empty<AccountRowViewModel>());
      }
    }

  }
}
