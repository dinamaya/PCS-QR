using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly MainDbContext _context;

		public CustomerRepository(MainDbContext context)
		{
			_context = context;
		}

		public async Task<Customer> CreateCustomerAsync(Customer customer)
		{
			customer.DateCreated = DateTime.UtcNow.ToLocalTime();
			customer.DateModified = DateTime.UtcNow.ToLocalTime();
			customer.IsActive = true;
			customer.ModifiedBy = string.Empty;

			_context.Customers.Add(customer);
			await _context.SaveChangesAsync();
			return customer;
		}

		public async Task<bool> CustomerExistsAsync(string email)
		{
			return await _context.Customers.AnyAsync(c => c.Email == email && c.IsActive);
		}
	}
}