using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.Controllers
{
  public class CasesController : Controller
  {
    private readonly ICaseRepository _caseRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly ITransactionRepository _transRepo;

		public CasesController(ICaseRepository caseRepo, ICustomerRepository customerRepo, ITransactionRepository transRepo)
		{
			_caseRepo = caseRepo;
			_customerRepo = customerRepo;
			_transRepo = transRepo;
		}

    [Authorize, HttpGet]
		public async Task<IActionResult> Update(string id)
    {
      var _case = await _caseRepo.GetById(id);
      long caseId = long.Parse(_case.Id);
      var transactions = await _transRepo.GetAllByCaseId(caseId);

      var customer = await _customerRepo.GetById(_case.CustomerId);

      ViewData[Keys.ViewData.CUSTOMER] = customer;
      ViewData[Keys.ViewData.CASE] = _case;
      ViewData[Keys.ViewData.TRANSACTIONS] = transactions;

      return View();
    }
    
		[HttpGet]
		public async Task<IActionResult> Tracking(string? refNo)
    {
      try
      {
        if (!refNo.IsNullOrEmpty())
        {
          var _case = await _caseRepo.GetByCaseNumber(refNo);
          long caseId = long.Parse(_case.Id);
          var transactions = await _transRepo.GetAllByCaseId(caseId);

          var customer = await _customerRepo.GetById(_case.CustomerId);

          ViewData[Keys.ViewData.CUSTOMER] = customer;
          ViewData[Keys.ViewData.CASE] = _case;
          ViewData[Keys.ViewData.TRANSACTIONS] = transactions;
        }

        return View("Tracking", refNo);
      }
      catch (Exception ex)
      {
        ViewData[Keys.ViewData.ERROR] = ex.Message;
        return View("Tracking", refNo);
      }
    }

    [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(CaseDetailsViewModel caseDetails)
    {
      return View();
    }
  }
}
