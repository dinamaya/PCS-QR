using CCIMS.Web.Controllers;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using ClosedXML.Excel;
using Microsoft.IdentityModel.Tokens;

namespace CCIMS.Web.Repositories.Implementations
{
    public class ExportReportRepository : IExportReportRepository
    {
        private readonly ICaseRepository _caseRepo;

        public ExportReportRepository(ICaseRepository caseRepo)
        {
            _caseRepo = caseRepo;
        }

        public async Task<IEnumerable<CaseRowViewModel>> GetFilteredCasesAsync(ExportAllRequestDto request)
        {
            IEnumerable<CaseRowViewModel> allCases;

            // Parse date range if provided
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (!request.DateRange.IsNullOrEmpty())
            {
                var dates = ParseDateRange(request.DateRange);
                startDate = dates.StartDate;
                endDate = dates.EndDate;
            }

            // Apply the same filtering logic as the Cases action
            if (!request.Category.IsNullOrEmpty() && !request.CategoryValue.IsNullOrEmpty())
            {
                // When filtering by category, we don't filter by service partner
                allCases = await _caseRepo.GetByCategory(request.Category, request.CategoryValue, startDate, endDate);
            }
            else
            {
                // When not filtering by category, we can filter by service partner
                allCases = await _caseRepo.GetDataAged5DaysByServicePartner(request.ServicePartner, startDate, endDate);
            }

            // Apply search term filtering if provided
            if (!request.SearchTerm.IsNullOrEmpty())
            {
                allCases = FilterCasesBySearchTerm(allCases, request.SearchTerm);
            }

            return allCases;
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

        public IEnumerable<CaseRowViewModel> FilterCasesBySearchTerm(IEnumerable<CaseRowViewModel> cases, string searchTerm)
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

        public async Task<byte[]> ExportCasesToExcelAsync(IEnumerable<CaseRowViewModel> cases)
        {
            return await Task.Run(() =>
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Cases");

                    // Set up headers
                    SetupWorksheetHeaders(worksheet);

                    // Convert and add data
                    var exportData = cases.Select(ConvertToExportDto).ToList();
                    PopulateWorksheetData(worksheet, exportData);

                    // Style the worksheet
                    StyleWorksheet(worksheet, exportData.Count);

                    // Save to memory stream and return bytes
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            });
        }

        private void SetupWorksheetHeaders(IXLWorksheet worksheet)
        {
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
        }

        private void PopulateWorksheetData(IXLWorksheet worksheet, List<ExportCaseViewModelDto> exportData)
        {
            for (int i = 0; i < exportData.Count; i++)
            {
                var row = exportData[i];
                worksheet.Cell(i + 2, 1).Value = row.CaseNumber;
                worksheet.Cell(i + 2, 2).Value = row.Status;
                worksheet.Cell(i + 2, 3).Value = row.Comments;
                worksheet.Cell(i + 2, 4).Value = row.CustomerName;
                worksheet.Cell(i + 2, 5).Value = row.ServicePartner;
                worksheet.Cell(i + 2, 6).Value = row.SerialNumber;
                worksheet.Cell(i + 2, 7).Value = row.DateCreated;
            }
        }

        private void StyleWorksheet(IXLWorksheet worksheet, int dataCount)
        {
            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Add borders to all cells
            if (dataCount > 0)
            {
                var dataRange = worksheet.Range(1, 1, dataCount + 1, 7);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }
        }

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
    }
}