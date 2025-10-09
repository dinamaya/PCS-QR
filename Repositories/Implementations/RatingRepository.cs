using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
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
    }
}