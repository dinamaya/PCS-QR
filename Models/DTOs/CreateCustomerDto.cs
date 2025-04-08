using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
	public class CreateCustomerDto
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string ContactNumber { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Address1 { get; set; }

		public string? Address2 { get; set; }

		[Required]
		public string Province { get; set; }

		[Required]
		public string CityMunicipality { get; set; }

		[Required]
		public string Barangay { get; set; }

		[Required]
		public string SerialNumber { get; set; }

		[Required]
		public string Token { get; set; }
	}
}
