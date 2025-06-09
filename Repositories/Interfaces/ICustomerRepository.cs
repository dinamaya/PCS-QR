using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ICustomerRepository : IEditRepository<CustomerEditRequestDto>
  {
    Task<string> CreateCustomerCaseAsync(CreateCustomerDto createCustomerDto);
    Task<bool> CustomerExistsAsync(string email);
    Task<CustomerDetailsViewModel> GetById(string id);
  }
}