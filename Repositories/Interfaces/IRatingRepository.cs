using CCIMS.Web.Models.Entities.Main;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task AddRatingAsync(Rating rating);
    }
}