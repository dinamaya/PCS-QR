using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Models.ViewModels
{
  public class CustomerEmailDetailsViewModel : EmailAssetViewModel
  {
    public string Email { get; set; }
    public string ServicePartner { get; set; }
    public string Fullname { get; set; }
    public string SerialNumber { get; set; }

    public CustomerEmailDetailsViewModel(IConfigurationRepository configRepo, string caseNumber, string encryptedCaseNumber) : base(configRepo, caseNumber, encryptedCaseNumber)
    {
    }
  }
}
