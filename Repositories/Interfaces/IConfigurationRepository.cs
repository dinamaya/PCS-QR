using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Repositories.Interfaces
{
	public interface IConfigurationRepository
	{
		string GetQrScanUrl();
		string GetPSGCBaseUrl();
		string GetPSGCProvines();
		string GetPSGCCitiesByProvinceCode(string code);
		string GetPSGCBarangaysByCityCode(string code);
		ISysAdminSecurityDetails GetSysAdminPrivateDetails();
  }
}
