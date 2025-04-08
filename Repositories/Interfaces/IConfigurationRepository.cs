using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IConfigurationRepository
	{
		string GetDocumentationUrl();
		string GetQrScanUrl();
		string GetPSGCBaseUrl();
		string GetPSGCProvinces();
		string GetPSGCCitiesByProvinceCode(string code);
		string GetPSGCBarangaysByCityCode(string code);
		ISysAdminSecurityDetails GetSysAdminPrivateDetails();
  }
}
