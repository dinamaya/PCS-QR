using CCIMS.Web.Models.Interfaces;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IConfigurationRepository
	{
		string GetBaseUrl();
    string GetDocumentationUrl();
		string GetQrScanUrl();
		string GetPSGCBaseUrl();
		string GetPSGCProvinces();

		IEnumerable<DropdownOptionViewModel> GetAgedSearcOptions();
		int GetAgedKeyByValue(string value);

    IEnumerable<DropdownOptionViewModel> GetCategoriesSearcOptions();

		IEnumerable<string> GetAllowedEmailDomains();

    string GetPSGCCitiesByProvinceCode(string code);
		string GetPSGCBarangaysByCityCode(string code);
		string GetTesseractTrainingDataPath();
		ISysAdminSecurityDetails GetSysAdminPrivateDetails();
  }
}
