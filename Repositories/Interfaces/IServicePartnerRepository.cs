using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IServicePartnerRepository : ICreateRepository<ServicePartner>
	{
		Task<string> GetId();
    Task<IEnumerable<ServicePartnerRowViewModel>> GetAll();
	}
}
