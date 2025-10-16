using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Models.ViewModels
{
    public class CaseStatusUpdateEmailViewModel : EmailAssetViewModel
    {
         public string Fullname { get; set; }
        public string ServicePartner { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string SerialNumber { get; set; }

        public CaseStatusUpdateEmailViewModel(IConfigurationRepository configRepo, string caseNumber, string encryptedCaseNumber) : base(configRepo, caseNumber, encryptedCaseNumber)
        {
        }
    }
}

