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
		private readonly ISecurityRepository _secureRepo;

    public BackgroundJobsService(ILogger<BackgroundJobsService> logger, IEmailService emailService, ICaseRepository caseRepo, IConfigurationRepository configRepo, ISecurityRepository secureRepo)
    {
      _logger = logger;
      _emailService = emailService;
      _caseRepo = caseRepo;
      _configRepo = configRepo;
      _secureRepo = secureRepo;
    }

    public async Task SendCasesAgedEmail()
    {
      var byCaseNumber = _configRepo.GetCategoriesSearcOptions().Where(c => c.Label.Equals("Case Number / ID")).Select(c => c.Value).FirstOrDefault();

      var cases = await _caseRepo.GetAgedCases(byCaseNumber);
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
      string encCaseNumber = await _secureRepo.EncryptIDAsync(caseDetails.CaseNumber);
      var emailDetails = new CustomerEmailDetailsViewModel(_configRepo, caseDetails.CaseNumber, encCaseNumber)
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

    public async Task SendCaseCreateEmail(CreateCustomerDto emailDetails, string caseNumber, string encryptedCaseNumber)
    {
      var result = await _emailService.SendCustomerRegistrationNotificationAsync(emailDetails, caseNumber, encryptedCaseNumber);

      if (result.IsOk())
        _logger.LogInformation(result.Message);
      else
        _logger.LogError(result.Message);
    }
  }
}
