using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Repositories.Implementations
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly MainDbContext _mainDb;


		public string InsertedId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public Task CreateAsync(Customer data)
		{
			throw new NotImplementedException();
		}
	}
}
