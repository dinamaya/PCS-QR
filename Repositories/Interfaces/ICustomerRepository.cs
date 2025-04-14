using CCIMS.Web.Models.Entities.Main;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface ICustomerRepository
	{
		Task<Customer> CreateCustomerAsync(Customer customer);
		Task<bool> CustomerExistsAsync(string email); // Optional: to check for duplicates
	}
}
