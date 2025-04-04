using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Models.Complex;
using CCIMS.Web.Models.Interfaces;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Repositories.Implementations
{
	public class ConfigurationRepository : IConfigurationRepository
	{
		private readonly IConfiguration _config;

		public ConfigurationRepository(IConfiguration config)
		{
			_config = config;
		}

		public string GetPSGCBaseUrl()
		{
			var url = _config.GetValue<string>("ApiConfig:external:psgc");
			if (string.IsNullOrWhiteSpace(url))
				throw new InvalidCastException(Exceptions.Message.Config.INVALID_APIURL_PSGC);

			return url;
		}
		public string GetPSGCProvinces() => Utils.Urls.Combine(GetPSGCBaseUrl(), "provinces");

		public string GetPSGCCitiesByProvinceCode(string code) => 
			Utils.Urls.Combine(GetPSGCProvinces(), $"provinces/{code}/cities-municipalities");

		public string GetPSGCBarangaysByCityCode(string code) => 
			Utils.Urls.Combine(GetPSGCBaseUrl(), $"cities-municipalities/{code}/barangays");

		public string GetPSGCCitiesByNCRRegion(string code) =>
			Utils.Urls.Combine(GetPSGCBaseUrl(), $"regions/1300000000/cities-municipalities");

		public ISysAdminSecurityDetails GetSysAdminPrivateDetails() => _config.GetSection("AdminSecurityConfig:Private").Get<SysAdminSecurityDetails>() ?? throw new InvalidCastException(Exceptions.Message.Config.INVALID_SYS_SECDETAILS);
  }
}
