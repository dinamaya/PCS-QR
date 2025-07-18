using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Models.ViewModels
{
  public class SpaCaseCreationEmailDetailsViewModel : EmailAssetViewModel
  {
    public string ServicePartner { get; set; }
    public string CustomerName { get; set; }
    public string SerialNumber { get; set; }

    public SpaCaseCreationEmailDetailsViewModel(IConfigurationRepository configRepo, string caseNumber) : base(configRepo, caseNumber, "")
    {
    }
  }
}
