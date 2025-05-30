using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using System.Text.Json;

using Hangfire;
using NuGet.Protocol.Core.Types;
using CCIMS.Web.App_Code._Globals;

namespace CCIMS.Web.Services.Implementations
{
	public class BackgroundJobsService : IBackgroundJobsService
  {
		private readonly ILogger<BackgroundJobsService> _logger;
    private readonly IEmailService _emailService;
    private readonly ICaseRepository _caseRepo;
    private readonly IConfigurationRepository _configRepo;

    public BackgroundJobsService(ILogger<BackgroundJobsService> logger, IEmailService emailService, ICaseRepository caseRepo, IConfigurationRepository configRepo)
    {
      _logger = logger;
      _emailService = emailService;
      _caseRepo = caseRepo;
      _configRepo = configRepo;
    }

    public async Task ExecuteAsync()
		{
      var result = await _caseRepo.GetAgedCases();
      var agedCases = new CaseAgedEmailDetailsViewModel()
      {
        BaseUrl = _configRepo.GetBaseUrl(),
        Cases = result
      };
      _logger.LogTrace("Aged Cases Emailed");
      await _emailService.SendAgedCasesEmailAsync(agedCases);
    }

    public async Task TestExecuteAsync()
    {
      _logger.LogTrace("Test Execute");
    }
  }
}
