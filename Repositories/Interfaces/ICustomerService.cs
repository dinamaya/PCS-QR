using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using System.Threading.Tasks;

namespace CCIMS.Web.Services.Interfaces
{
	public interface ICustomerService
	{
		Task<ProvincesViewModel> GetRegisterViewModelAsync(string token);
		Task<bool> RegisterCustomerAsync(CreateCustomerDto createCustomerDto);
	}
}