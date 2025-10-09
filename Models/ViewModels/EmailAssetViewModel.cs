using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;

namespace CCIMS.Web.Models.ViewModels
{
  public class EmailAssetViewModel
  {
    public string BaseUrl { get; }
    public string CaseNumber { get; set; }
    public string CaseTrackingLink { get; }
    public string FeedbackFormLink { get; }

    public EmailAssetViewModel(IConfigurationRepository configRepo, string caseNumber, string encryptedCaseNumber)
    {
      var _uri = new Uri(configRepo.GetBaseUrl());
      BaseUrl = _uri.AbsoluteUri;
      CaseNumber = caseNumber;
      CaseTrackingLink = new Uri(_uri, $"Cases/Tracking?refNo={encryptedCaseNumber}").AbsoluteUri;
      FeedbackFormLink = new Uri(_uri, $"Customer/Feedback?refNo={encryptedCaseNumber}").AbsoluteUri;
    }
  }
}
