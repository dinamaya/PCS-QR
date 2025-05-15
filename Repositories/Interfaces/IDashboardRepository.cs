using CCIMS.Web.Models.Entities;
using CCIMS.Web.Models.SQLViews.Main;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IDashboardRepository
	{
		Task<int> GetAgingCasesCountAsync();
		Task<int> GetClosedCasesCountAsync();
		Task<int> GetCasesCreatedTodayCountAsync();
		Task<int> GetTotalCasesCountAsync();
		Task<List<WeeklyCasesChartV>> GetWeeklyCasesChartAsync();
		Task<List<WeeklyAgingCasesChartV>> GetWeeklyAgingCasesChartAsync();
		Task<List<LatestCasesSubmissionV>> GetLatestCaseSubmissionsAsync();
		Task<List<Top3ServicePartnersAgingCasesV>> GetTop3ServicePartnersAgingCases();
	}
}
