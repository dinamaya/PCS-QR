using CCIMS.Web.Models.DTOs;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task CreateCustomerCaseAsync(CreateCustomerDto createCustomerDto);
        Task<bool> CustomerExistsAsync(string email);
    }
}