using CCIMS.Web.App_Code._Globals;
using Microsoft.AspNetCore.Hosting.Server;

namespace CCIMS.Web.Models.ViewModels
{
  public class CustomerEmailAssetsViewModel : CustomerEmailDetailsViewModel
  {
    private readonly Uri imgFolder;
    private readonly Uri icnFolder;
    public Uri BaseUrl { get; }
    public string WebIcon { get; }
    public string VstIcon { get; }
    public string CheckIcon { get; }
    public string CharacterImage { get; }
    public string CaseTrackingLink { get; }

    public CustomerEmailAssetsViewModel(Server server, CustomerEmailDetailsViewModel emailDetails)
    {
      BaseUrl = server.BaseUrl;
      imgFolder = new Uri(BaseUrl, "img/");
      icnFolder = new Uri(imgFolder, "icons/");
      WebIcon = new Uri(icnFolder, "icon_ccims_lg.svg").AbsoluteUri;
      VstIcon = new Uri(icnFolder, "VST-ECS.png").AbsoluteUri;
      CheckIcon = new Uri(icnFolder, "circle-check-solid.png").AbsoluteUri;
      CharacterImage = new Uri(imgFolder, "illustrations/thank-you1.png").AbsoluteUri;
      CaseTrackingLink = new Uri(BaseUrl, $"Cases/Tracking?refNo={emailDetails.CaseNumber}").AbsoluteUri;

      base.Email = emailDetails.Email;
      base.CaseNumber = emailDetails.CaseNumber;
      base.ServicePartner = emailDetails.ServicePartner;
      base.Fullname = emailDetails.Fullname;
    }
  }
}
