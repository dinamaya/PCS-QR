using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Auth;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IAccountRepository : IEditRepository<AccountEditRequestDto>
	{
		Task CreateAsync(AccountCreationRequestDto creationRequest, string createdBy);
		Task<IEnumerable<AccountRowViewModel>> GetAll();
		Task<IEnumerable<DropdownOptionViewModel>> GetAllRoles();
		Task<AccountEditResponseDto> GetById(string id);
	}
}
