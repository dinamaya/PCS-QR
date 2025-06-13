using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models;
using CCIMS.Web.Repositories.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		private readonly ILogger<DashboardController> _logger;
		private readonly IDashboardRepository _dashboardRepo;

		public DashboardController(ILogger<DashboardController> logger, IDashboardRepository dashboardRepo)
		{
			_logger = logger;
			_dashboardRepo = dashboardRepo;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.AgingCasesCount = await _dashboardRepo.GetAgingCasesCountAsync();
			ViewBag.ClosedCasesCount = await _dashboardRepo.GetClosedCasesCountAsync();
			ViewBag.CasesCreatedToday = await _dashboardRepo.GetCasesCreatedTodayCountAsync();
			ViewBag.TotalCasesCount = await _dashboardRepo.GetTotalCasesCountAsync();
			ViewBag.WeeklyCasesChart = await _dashboardRepo.GetWeeklyCasesChartAsync();
			ViewBag.WeeklyClosedCasesChart = await _dashboardRepo.GetWeeklyClosedCasesChartAsync();
			ViewBag.LatestCaseSubmission = await _dashboardRepo.GetLatestCaseSubmissionsAsync();
			ViewBag.Top3ServicePartnersAgingCases = await _dashboardRepo.GetTop3ServicePartnersAgingCases();
            ViewBag.TopAgingCases = await _dashboardRepo.GetTopAgingCases();


            return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
