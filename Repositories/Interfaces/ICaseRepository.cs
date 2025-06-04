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
		Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value);

		Task<CaseDetailsViewModel> GetById(string id);
        Task<string> GetCurrentStatus(long caseId);
        Task<CaseDetailsViewModel> GetByCaseNumber(string caseNumber);
        string GenerateCaseNumber();
        Task EditAsync(CaseEditRequestDto editRequestDto, string modifiedBy);

        Task<IEnumerable<CaseRowViewModel>> GetDataAged5DaysByServicePartner(string spName);

		Task<IEnumerable<CaseRowViewModel>> GetFilteredCasesByServicePartner(string spName, DateTime? startDate = null, DateTime? endDate = null);
		Task<IEnumerable<CaseRowViewModel>> GetFilteredCasesByCategory(string categoryId, string value, DateTime? startDate = null, DateTime? endDate = null);

	}
}