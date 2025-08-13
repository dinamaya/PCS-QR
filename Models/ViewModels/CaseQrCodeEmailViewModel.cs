using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Models.ViewModels
{
    public class CaseQrCodeEmailViewModel
    {
        public string BaseUrl { get; }
        public string Fullname { get; set; }
        public string Spname { get; set; }
        public string Email { get; set; }
        public string QrCodeContentId { get; set; }

        public CaseQrCodeEmailViewModel(IConfigurationRepository configRepo)
        {
            BaseUrl = configRepo.GetBaseUrl();
        }
    }
}
