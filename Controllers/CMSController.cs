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
        public async Task<IActionResult> Cases(string? c = null, string? v = null, string? d = null)
        {
            try
            {
                await InitializeValues();
                IEnumerable<CaseRowViewModel>? results = null;

                if (!c.IsNullOrEmpty() && !v.IsNullOrEmpty())
                {
                    if (d.IsNullOrEmpty())
                        results = await _caseRepo.GetByCategory(c, v);
                    else
                        results = null;
                }
                else
                    results = await _caseRepo.GetDataAged5DaysByServicePartner(d);

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

        // NEW METHOD: Export all data based on current filters
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

    // DTO for filter properties from the View
    public class ExportAllRequestDto
    {
        public string? Category { get; set; }
        public string? CategoryValue { get; set; }
        public string? ServicePartner { get; set; }
        public string? SearchTerm { get; set; }
        // Add any other filter properties you need
    }
}