using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.SQLViews.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;

namespace CCIMS.Web.Services.Implementations
{
	public class BackgroundJobsService
  {
		private readonly ILogger<BackgroundJobsService> _logger;
		private readonly IEmailService _emailService;
		private readonly IConfigurationRepository _configRepo;
		private readonly ICaseRepository _caseRepo;

    public BackgroundJobsService(ILogger<BackgroundJobsService> logger, IEmailService emailService, ICaseRepository caseRepo, IConfigurationRepository configRepo)
    {
      _logger = logger;
      _emailService = emailService;
      _caseRepo = caseRepo;
      _configRepo = configRepo;
    }

    public async Task TestEmailAsync()
    {
      var result = await _caseRepo.GetAgedCases();
      var agedCases = new CaseAgedEmailDetailsViewModel()
      {
        BaseUrl = _configRepo.GetBaseUrl(),
        Cases = result
      };
      
      _logger.LogInformation("Aged Cases");

      foreach (var _case in result)
        _logger.LogInformation(_case.CaseNumber);

      _logger.LogInformation("===============");
    }

    public async Task SendCasesAgedEmail()
    {
      var cases = await _caseRepo.GetAgedCases();
      var agedCases = new CaseAgedEmailDetailsViewModel()
      {
        BaseUrl = _configRepo.GetBaseUrl(),
        Cases = cases
      };

      var result = await _emailService.SendAgedCasesEmailAsync(agedCases);

      if (result.IsOk())
        _logger.LogInformation(result.Message);
      else
        _logger.LogError(result.Message);
    }

    public async Task SendCaseClosedEmail(CaseDetailsV caseDetails)
    {
      var emailDetails = new CustomerEmailDetailsViewModel(_configRepo, caseDetails.CaseNumber)
      {
        Email = caseDetails.Email,
        Fullname = $"{caseDetails.FirstName} {caseDetails.LastName}",
        ServicePartner = caseDetails.SpName,
      };

      var result = await _emailService.SendCaseClosedNotificationAsync(emailDetails);

      if (result.IsOk())
        _logger.LogInformation(result.Message);
      else
        _logger.LogError(result.Message);
    }

    public async Task SendCaseCreateEmail(CreateCustomerDto emailDetails, string caseNumber)
    {
      var result = await _emailService.SendCustomerRegistrationNotificationAsync(emailDetails, caseNumber);

      if (result.IsOk())
        _logger.LogInformation(result.Message);
      else
        _logger.LogError(result.Message);
    }
  }
}
