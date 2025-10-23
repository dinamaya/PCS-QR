using CCIMS.Web.App_Code._Globals;
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
    private readonly ISecurityRepository _secRepo;
    private readonly IRatingRepository _ratingRepo;

    public CasesController(ICaseRepository caseRepo, ICustomerRepository customerRepo, ITransactionRepository transRepo, ISecurityRepository secRepo, IRatingRepository ratingRepo)
        {
            _caseRepo = caseRepo;
            _customerRepo = customerRepo;
            _transRepo = transRepo;
            _secRepo = secRepo;
            _ratingRepo = ratingRepo;
        }

        [Authorize(Roles = "SPA"), HttpGet]
    public async Task<IActionResult> Update(string id, string? q = null)
    {
      try
      {
        if (!q.IsNullOrEmpty())
        {
          if (q.Equals(Queries.SUCCESS_EDIT)) ViewData[Keys.ViewData.SUCCESS] = "Customer Edited Successfully";
        }

        var _case = await _caseRepo.GetById(id);
        long caseId = long.Parse(_case.Id);
        var transactions = await _transRepo.GetAllByCaseId(caseId);
        //var rating = await _ratingRepo.GetByCaseIdAsync(caseId);

        var customer = await _customerRepo.GetById(_case.CustomerId);

        ViewData[Keys.ViewData.CUSTOMER] = customer;
        ViewData[Keys.ViewData.CASE] = _case;
        ViewData[Keys.ViewData.TRANSACTIONS] = transactions;
        //ViewData[Keys.ViewData.RATING] = rating;

        return View();
      }
      catch (Exception ex)
      {
        return RedirectToAction("Index", "Dashboard");
      }
    }
    
		[HttpGet]
		public async Task<IActionResult> Tracking(string? refNo)
    {
      string decRefNo = "";
      try
      {
        if(!refNo.IsNullOrEmpty())
          decRefNo = await _secRepo.DecryptIDAsync(refNo);
      }
      catch (Exception ex)
      {
        ViewData[Keys.ViewData.ERROR] = ex.Message;
        return View("Tracking", decRefNo);
      }

      try
      {
        if (!refNo.IsNullOrEmpty())
        {

          var _case = await _caseRepo.GetByCaseNumber(decRefNo);
          long caseId = long.Parse(_case.Id);
          var transactions = await _transRepo.GetAllByCaseId(caseId);

          var customer = await _customerRepo.GetById(_case.CustomerId);

          ViewData[Keys.ViewData.CUSTOMER] = customer;
          ViewData[Keys.ViewData.CASE] = _case;
          ViewData[Keys.ViewData.TRANSACTIONS] = transactions;
        }

        return View("Tracking", decRefNo);
      }
      catch (Exception ex)
      {
        ViewData[Keys.ViewData.ERROR] = ex.Message;
        return View("Tracking", decRefNo);
      }
    }

        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(CaseDetailsViewModel caseDetails)
    {
      return View();
    }
  }
}
