using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ClosedXML.Excel;
using System.Linq;
using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Controllers
{
  public class CMSController : Controller
  {
    private readonly IServicePartnerRepository _spRepo;
    private readonly IAccountRepository _accountRepo;
    private readonly IOperationsRepository _opsRepo;
    private readonly ICaseRepository _caseRepo;
    private readonly IConfigurationRepository _configRepo;
    private readonly IExportReportRepository _exportRepo;

    public CMSController(IServicePartnerRepository spRepo, IAccountRepository accountRepo, IOperationsRepository opsRepo, IConfigurationRepository configRepo, ICaseRepository caseRepo,
        IExportReportRepository exportRepo)
    {
      _spRepo = spRepo;
      _accountRepo = accountRepo;
      _opsRepo = opsRepo;
      _configRepo = configRepo;
      _caseRepo = caseRepo;
      _exportRepo = exportRepo;
    }

    [Authorize]
    public IActionResult Index()
    {
      return View();
    }

    [Authorize(Roles = "SPA")]
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

    [Authorize(Roles = "OPS")]
    public async Task<IActionResult> Account(string? q)
    {
      try
      {
        if (!q.IsNullOrEmpty())
        {
          if (q.Equals(Queries.SUCCESS_CREATE)) ViewData[Keys.ViewData.SUCCESS] = "Account Created Successfully";
          if (q.Equals(Queries.SUCCESS_EDIT)) ViewData[Keys.ViewData.SUCCESS] = "Account Edited Successfully";
          if (q.Equals(Queries.SUCCESS_DELETE)) ViewData[Keys.ViewData.SUCCESS] = "Account Deleted Successfully";
        }

        var results = await _accountRepo.GetAll();
        ViewData[Keys.ViewData.Types.ROLES] = await _accountRepo.GetAllRoles();

        return View(results);
      }
      catch (Exception ex)
      {
        return View(Enumerable.Empty<AccountRowViewModel>());
      }
    }

    [Authorize(Roles = "SPA")]
    public async Task<IActionResult> Cases(string? c = null, string? v = null, string? d = null, string? dateRange = null)
    {
      try
      {
        await InitializeValues();
        IEnumerable<CaseRowViewModel>? results = Enumerable.Empty<CaseRowViewModel>();

        // Parse date range if provided
        DateTime? startDate = DateTime.MinValue;
        DateTime? endDate = DateTime.MaxValue;
        if (!dateRange.IsNullOrEmpty())
        {
          var dates = ParseDateRange(dateRange);
          startDate = dates.StartDate;
          endDate = dates.EndDate;
        }

        if (!c.IsNullOrEmpty() && !v.IsNullOrEmpty())
          results = await _caseRepo.GetByCategory(c, v, startDate, endDate);

        return View(results);
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

    private (DateTime? StartDate, DateTime? EndDate) ParseDateRange(string dateRange)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(dateRange))
          return (null, null);

        // Handle flatpickr range format: "2023-01-01 to 2023-01-31"
        var parts = dateRange.Split(" to ");
        if (parts.Length == 2)
        {
          if (DateTime.TryParse(parts[0].Trim(), out DateTime start) &&
              DateTime.TryParse(parts[1].Trim(), out DateTime end))
          {
            return (start.Date, end.Date.AddDays(1).AddSeconds(-1)); // Include end of day
          }
        }

        // Handle single date
        if (DateTime.TryParse(dateRange.Trim(), out DateTime singleDate))
        {
          return (singleDate.Date, singleDate.Date.AddDays(1).AddSeconds(-1));
        }

        return (null, null);
      }
      catch
      {
        return (null, null);
      }
    }

    // Updated Export method with date range support
    [Authorize(Roles = "SPA")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Export([FromBody] ExportAllRequestDto request)
    {
      try
      {
        // Get filtered cases using the repository
        var filteredCases = await _exportRepo.GetFilteredCasesAsync(request);

        // Generate Excel file using the repository
        var excelBytes = await _exportRepo.ExportCasesToExcelAsync(filteredCases);

        // Generate filename and return file
        var fileName = $"Cases_Export_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
      }
      catch (Exception ex)
      {
        return BadRequest("Error exporting all cases to Excel: " + ex.Message);
      }
    }
  }
}