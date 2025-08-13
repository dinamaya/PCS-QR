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
        private readonly ISecurityRepository _secureRepo;

        public DashboardController(ILogger<DashboardController> logger, IDashboardRepository dashboardRepo, ISecurityRepository secureRepo)
        {
            _logger = logger;
            _dashboardRepo = dashboardRepo;
            _secureRepo = secureRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AgingCasesCount = await _dashboardRepo.GetAgingCasesCountAsync();
            ViewBag.ClosedCasesCount = await _dashboardRepo.GetClosedCasesCountAsync();
            ViewBag.CasesCreatedToday = await _dashboardRepo.GetCasesCreatedTodayCountAsync();
            ViewBag.TotalCasesCount = await _dashboardRepo.GetTotalCasesCountAsync();
            ViewBag.WeeklyCasesChart = await _dashboardRepo.GetWeeklyCasesChartAsync();
            ViewBag.WeeklyClosedCasesChart = await _dashboardRepo.GetWeeklyClosedCasesChartAsync();
            ViewBag.Top3ServicePartnersAgingCases = await _dashboardRepo.GetTop3ServicePartnersAgingCases();
            var latestCaseSubmission = await _dashboardRepo.GetLatestCaseSubmissionsAsync();
            var topAgingCases = await _dashboardRepo.GetTopAgingCases();

            if (User.IsInRole("OPS"))
            {
                foreach (var _case in latestCaseSubmission)
                    _case.CaseNumber = await _secureRepo.EncryptIDAsync(_case.CaseNumber);

                foreach (var _case in topAgingCases)
                    _case.CaseNumber = await _secureRepo.EncryptIDAsync(_case.CaseNumber);
            }

            ViewBag.LatestCaseSubmission = latestCaseSubmission;
            ViewBag.TopAgingCases = topAgingCases;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
