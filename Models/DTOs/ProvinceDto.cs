using System.Text.Json.Serialization;

namespace CCIMS.Web.Models.DTOs
{
	public class ProvinceDto
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("code")]
		public string Code { get; set; }

		[JsonPropertyName("region_id")]
		public int RegionId { get; set; }
	}
}