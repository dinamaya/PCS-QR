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

        public CMSController(IServicePartnerRepository spRepo, IAccountRepository accountRepo, IOperationsRepository opsRepo, IConfigurationRepository configRepo, ICaseRepository caseRepo)
        {
            _spRepo = spRepo;
            _accountRepo = accountRepo;
            _opsRepo = opsRepo;
            _configRepo = configRepo;
            _caseRepo = caseRepo;
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
                IEnumerable<CaseRowViewModel> allCases;

                // Apply the same filtering logic as the Cases action
                if (!request.Category.IsNullOrEmpty() && !request.CategoryValue.IsNullOrEmpty())
                {
                    if (request.ServicePartner.IsNullOrEmpty())
                        allCases = await _caseRepo.GetByCategory(request.Category, request.CategoryValue);
                    else
                        allCases = Enumerable.Empty<CaseRowViewModel>(); // Handle this case as needed
                }
                else
                {
                    allCases = await _caseRepo.GetDataAged5DaysByServicePartner(request.ServicePartner);
                }

                // Apply search term filtering if provided
                if (!request.SearchTerm.IsNullOrEmpty())
                {
                    allCases = FilterCasesBySearchTerm(allCases, request.SearchTerm);
                }

                // Convert CaseRowViewModel to ExportCaseViewModelDto
                var exportData = allCases.Select(ConvertToExportDto).ToList();

                return GenerateExcelFile(exportData);
            }
            catch (Exception ex)
            {
                return BadRequest("Error exporting all cases to Excel: " + ex.Message);
            }
        }

        // HELPER METHOD: Convert CaseRowViewModel to ExportCaseViewModelDto
        private ExportCaseViewModelDto ConvertToExportDto(CaseRowViewModel caseRow)
        {
            return new ExportCaseViewModelDto
            {
                CaseNumber = caseRow.CaseNumber ?? string.Empty,
                Status = caseRow.Status ?? string.Empty,
                Comments = caseRow.Comments ?? string.Empty,
                CustomerName = caseRow.CustomerName ?? string.Empty,
                ServicePartner = caseRow.ServicePartnerName ?? string.Empty,
                SerialNumber = caseRow.SerialNumber ?? string.Empty,
                DateCreated = caseRow.DateCreated ?? string.Empty
            };
        }

        // HELPER METHOD: Filter cases by search term (mimics DataTable search behavior)
        private IEnumerable<CaseRowViewModel> FilterCasesBySearchTerm(IEnumerable<CaseRowViewModel> cases, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return cases;

            var searchTermLower = searchTerm.Trim().ToLowerInvariant();

            return cases.Where(c =>
                (!string.IsNullOrEmpty(c.CaseNumber) && c.CaseNumber.ToLowerInvariant().Contains(searchTermLower)) ||
                (!string.IsNullOrEmpty(c.Status) && c.Status.ToLowerInvariant().Contains(searchTermLower)) ||
                (!string.IsNullOrEmpty(c.Comments) && c.Comments.ToLowerInvariant().Contains(searchTermLower)) ||
                (!string.IsNullOrEmpty(c.CustomerName) && c.CustomerName.ToLowerInvariant().Contains(searchTermLower)) ||
                (!string.IsNullOrEmpty(c.ServicePartnerName) && c.ServicePartnerName.ToLowerInvariant().Contains(searchTermLower)) ||
                (!string.IsNullOrEmpty(c.SerialNumber) && c.SerialNumber.ToLowerInvariant().Contains(searchTermLower)) ||
                (!string.IsNullOrEmpty(c.DateCreated) && c.DateCreated.ToLowerInvariant().Contains(searchTermLower))
            );
        }

        // HELPER METHOD: Generate Excel file
        private IActionResult GenerateExcelFile(List<ExportCaseViewModelDto> exportCases)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Cases");

                // Set up headers
                worksheet.Cell(1, 1).Value = "Case Number";
                worksheet.Cell(1, 2).Value = "Status";
                worksheet.Cell(1, 3).Value = "Comments / Remarks";
                worksheet.Cell(1, 4).Value = "Customer Name";
                worksheet.Cell(1, 5).Value = "Service Partner";
                worksheet.Cell(1, 6).Value = "Serial Number";
                worksheet.Cell(1, 7).Value = "Date Created";

                // Style headers
                var headerRange = worksheet.Range("A1:G1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Add data
                for (int i = 0; i < exportCases.Count; i++)
                {
                    var row = exportCases[i];
                    worksheet.Cell(i + 2, 1).Value = row.CaseNumber;
                    worksheet.Cell(i + 2, 2).Value = row.Status;
                    worksheet.Cell(i + 2, 3).Value = row.Comments;
                    worksheet.Cell(i + 2, 4).Value = row.CustomerName;
                    worksheet.Cell(i + 2, 5).Value = row.ServicePartner;
                    worksheet.Cell(i + 2, 6).Value = row.SerialNumber;
                    worksheet.Cell(i + 2, 7).Value = row.DateCreated;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Add borders to all cells
                if (exportCases.Count > 0)
                {
                    var dataRange = worksheet.Range(1, 1, exportCases.Count + 1, 7);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                // Save to memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = $"Cases_Export_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
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