using CCIMS.Web.Models.Entities.HangFire;
using Microsoft.EntityFrameworkCore;

namespace CCIMS.Web.Context
{
	public class HangfireDbContext(DbContextOptions<HangfireDbContext> options) : DbContext(options)
	{
		public virtual DbSet<NonWorkingDate> NonWorkingDates { get; set; }
	}
}
