using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories
{
	public class CaseRepository : ICaseRepository
	{
		private readonly MainDbContext _context;

		public CaseRepository(MainDbContext context)
		{
			_context = context;
		}

		public string InsertedId { get; set; }

		public async Task CreateAsync(Case newCase, string createdBy)
		{
			newCase.CaseNumber = Guid.NewGuid().ToString();
			newCase.DateCreated = DateTime.UtcNow.ToLocalTime();
			newCase.IsActive = true;
			newCase.ModifiedBy = string.Empty;
			newCase.Description = string.Empty;

			_context.Cases.Add(newCase);
			await _context.SaveChangesAsync();
		}

		public Task<IEnumerable<CaseRowViewModel>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<CaseRowViewModel> GetById(string id)
		{
			throw new NotImplementedException();
		}
	}
}