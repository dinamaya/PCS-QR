using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ICaseRepository : IReadOnlyRepository<CaseRowViewModel, CaseDetailsViewModel>, ICreateRepository<Case>
	{
		Task<IEnumerable<CaseRowViewModel>> GetByCategory(string categoryId, string value);
		Task<string> GetCurrentStatus(long caseId);
		Task<CaseDetailsViewModel> GetByCaseNumber(string caseNumber);
  }
}
