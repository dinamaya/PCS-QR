using CCIMS.Web.Models.Entities.Main;

namespace CCIMS.Web.Repositories.Interfaces
{
	namespace CCIMS.Web.Repositories.Interfaces
	{
		public interface ICaseRepository
		{
			Task<Case> CreateCaseAsync(Case newCase);
		}
	}
}
