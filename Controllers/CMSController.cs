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
    private readonly IOperationsRepository _opsRepo;
    private readonly IConfigurationRepository _configRepo;

    public CMSController(IServicePartnerRepository spRepo, IAccountRepository accountRepo, IOperationsRepository opsRepo, IConfigurationRepository configRepo)
    {
      _spRepo = spRepo;
      _accountRepo = accountRepo;
      _opsRepo = opsRepo;
      _configRepo = configRepo;
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

    public async Task<IActionResult> Cases(string? c = null, string? v = null)
    {
      try
      {
        await InitializeValues();
        if (c.IsNullOrEmpty() || v.IsNullOrEmpty())
          return View(Enumerable.Empty<CaseRowViewModel>());
        //var results = await _accountRepo.GetAll();
        
        return View(Enumerable.Empty<CaseRowViewModel>());
      }
      catch (Exception ex)
      {
        return View(Enumerable.Empty<CaseRowViewModel>());
      }
    }

    private async Task InitializeValues()
    {
      ViewData[Keys.ViewData.Types.CATEGORIES] = _configRepo.GetCategoriesSearcOptions();
     
      ViewData[Keys.ViewData.Types.STATUS] = await _opsRepo.GetOptions();
      ViewData[Keys.ViewData.Types.AGED] = _configRepo.GetAgedSearcOptions();
    }
  }
}
