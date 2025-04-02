using System.Text.Json.Serialization;

namespace CCIMS.Web.Models.DTOs
{
	public class CityMunicipalityDto
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("code")]
		public string Code { get; set; }

		[JsonPropertyName("province_id")]
		public int ProvinceId { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }
	}
}