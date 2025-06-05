using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ICaseRepository : 
		IReadOnlyRepository<CaseRowViewModel, CaseDetailsViewModel>, 
		ICreateRepository<Case>, 
		IEditRepository<CaseEditRequestDto>
	{
		Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value, DateTime? startDate, DateTime? endDate);
		Task<IEnumerable<AgedCaseViewModel>> GetAgedCases();
		Task<string> GetCurrentStatus(long caseId);
		Task<CaseDetailsViewModel> GetByCaseNumber(string caseNumber);
		string GenerateCaseNumber();
  }
}
