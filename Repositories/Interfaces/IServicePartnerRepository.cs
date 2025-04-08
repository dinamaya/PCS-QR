using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IServicePartnerRepository : ICreateRepository<ServicePartner>
	{
		Task<SPEditResponseDto> GetById(string id);
    Task<IEnumerable<ServicePartnerRowViewModel>> GetAll();
		Task EditAsync(SPEditRequestDto editRequestDto, string modifiedBy);
	}
}
