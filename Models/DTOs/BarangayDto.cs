using System.Text.Json.Serialization;

namespace CCIMS.Web.Models.DTOs
{
	public class BarangayDto
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("code")]
		public string Code { get; set; }

		[JsonPropertyName("city_mun_id")]
		public int CityMunId { get; set; }
	}
}
