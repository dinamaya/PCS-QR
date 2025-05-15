using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities;
using CCIMS.Web.Models.SQLViews.Main;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories
{
	public class DashboardRepository : IDashboardRepository
	{
		private readonly MainDbContext _context;
		private readonly ILogger<DashboardRepository> _logger;

		public DashboardRepository(MainDbContext context, ILogger<DashboardRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<int> GetAgingCasesCountAsync() => await _context.AgingCasesVs.CountAsync();

		public async Task<int> GetClosedCasesCountAsync() => await _context.ClosedCasesVs.CountAsync();

		public async Task<int> GetCasesCreatedTodayCountAsync() => await _context.CasesCreatedTodayVs.CountAsync();

		public async Task<int> GetTotalCasesCountAsync() => await _context.Cases.CountAsync();

		public async Task<List<WeeklyCasesChartV>> GetWeeklyCasesChartAsync() =>
			await _context.WeeklyCasesChartVs.ToListAsync();

		public async Task<List<WeeklyAgingCasesChartV>> GetWeeklyAgingCasesChartAsync() =>
			await _context.WeeklyAgingCasesChartVs.ToListAsync();

		public async Task<List<LatestCasesSubmissionV>> GetLatestCaseSubmissionsAsync() =>
			await _context.LatestCasesSubmissionVs.ToListAsync();

		public async Task<List<Top3ServicePartnersAgingCasesV>> GetTop3ServicePartnersAgingCases() =>
			await _context.Top3ServicePartnersAgingCasesVs.ToListAsync();
	}
}
