using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Models.ViewModels
{
	public class RegistrationViewModel
	{
		public IEnumerable<ProvinceDto> Provinces { get; set; } = Enumerable.Empty<ProvinceDto>();
		public IEnumerable<CityMunicipalityDto> CitiesMunicipalities { get; set; } 

		// Form fields
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ContactNumber { get; set; }
		public string Email { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Province { get; set; }
		public string CityMunicipality { get; set; }
		public string Barangay { get; set; }
		public string SerialNumber { get; set; }
	}
}
