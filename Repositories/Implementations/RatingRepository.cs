using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MainDbContext _context;

        public RatingRepository(MainDbContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RatingRowViewModel>> GetAllAsync()
        {
            var results = await (from r in _context.Ratings
                                 join c in _context.LatestCasesVs on r.CaseId equals c.CaseId
                                 select new RatingRowViewModel
                                 {
                                     Id = r.Id.ToString(),
                                     CaseId = r.CaseId.ToString(),
                                     CaseNumber = c.CaseNumber,
                                     Comment = r.Comment,
                                     CustomerId = r.CustomerId,
                                     CustomerName = c.CustomerFirstName + " " + c.CustomerLastName,
                                     DateCreated = r.DateCreated,
                                     RatingVal = r.RatingVal,
                                     SerialNumber = c.SerialNumber,
                                     ServicePartnerName = c.ServicePartnerName
                                 }).ToListAsync();
            return results;
        }
    }
}