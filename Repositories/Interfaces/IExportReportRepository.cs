
using CCIMS.Web.Controllers;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
    public interface IExportReportRepository
    {
        Task<byte[]> ExportCasesToExcelAsync(IEnumerable<CaseRowViewModel> cases);
        Task<IEnumerable<CaseRowViewModel>> GetFilteredCasesAsync(ExportAllRequestDto request);
        IEnumerable<CaseRowViewModel> FilterCasesBySearchTerm(IEnumerable<CaseRowViewModel> cases, string searchTerm);
    }
}