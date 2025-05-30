using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
    public interface ICaseRepository
    {
        string InsertedId { get; set; }

        Task CreateAsync(Case newCase, string createdBy);
        Task<IEnumerable<CaseRowViewModel>> GetAll();

        // Updated methods with date range support
        Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value);
        Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value, DateTime? startDate, DateTime? endDate);

        Task<CaseDetailsViewModel> GetById(string id);
        Task<string> GetCurrentStatus(long caseId);
        Task<CaseDetailsViewModel> GetByCaseNumber(string caseNumber);
        string GenerateCaseNumber();
        Task EditAsync(CaseEditRequestDto editRequestDto, string modifiedBy);

        // Updated methods with date range support
        Task<IEnumerable<CaseRowViewModel>> GetDataAged5DaysByServicePartner(string spName);
        Task<IEnumerable<CaseRowViewModel>> GetDataAged5DaysByServicePartner(string spName, DateTime? startDate, DateTime? endDate);
    }
}