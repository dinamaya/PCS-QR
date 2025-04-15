using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ICaseRepository : : IReadOnlyRepository<CaseRowViewModel, CaseRowViewModel>
	{
		Task<Case> CreateCaseAsync(Case newCase);
	}
}
