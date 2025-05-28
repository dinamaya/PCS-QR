using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;

namespace CCIMS.Web.Models.ViewModels
{
  public class CustomerEmailAssetsViewModel : CustomerEmailDetailsViewModel
  {
    public string BaseUrl { get; }
    public string CaseTrackingLink { get; }

    public CustomerEmailAssetsViewModel(IConfigurationRepository configRepo, CustomerEmailDetailsViewModel emailDetails)
    {
      base.Email = emailDetails.Email;
      base.CaseNumber = emailDetails.CaseNumber;
      base.ServicePartner = emailDetails.ServicePartner;
      base.Fullname = emailDetails.Fullname;

      var _uri = new Uri(configRepo.GetBaseUrl());
      BaseUrl = _uri.AbsoluteUri;
      CaseTrackingLink = new Uri(_uri, $"Cases/Tracking?refNo={CaseNumber}").AbsoluteUri;
    }
  }
}
