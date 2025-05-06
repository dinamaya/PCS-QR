using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models;
using CCIMS.Web.Models.ViewModels;
using System.Diagnostics;
using CCIMS.Web.Context;

namespace CCIMS.Web.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{

		private readonly ILogger<DashboardController> _logger;
		private readonly MainDbContext _context;

		public DashboardController(ILogger<DashboardController> logger, MainDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
      var agingCasesCount = _context.AgingCasesVs.Count();
			var closedCasesCount = _context.ClosedCasesVs.Count();
      var casesCreatedToday = _context.CasesCreatedTodayVs.Count();
      var totalCasesCount = _context.Cases.Count();
      var weeklyCasesChart = _context.WeeklyCasesChartVs.ToList();
      var weeklyAgingCasesChart = _context.WeeklyAgingCasesChartVs.ToList();

      ViewBag.AgingCasesCount = agingCasesCount;
			ViewBag.ClosedCasesCount = closedCasesCount;
			ViewBag.CasesCreatedToday = casesCreatedToday;
      ViewBag.TotalCasesCount = totalCasesCount;
      ViewBag.WeeklyCasesChart = weeklyCasesChart;
      ViewBag.WeeklyAgingCasesChart = weeklyAgingCasesChart;

      return View();
    }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

