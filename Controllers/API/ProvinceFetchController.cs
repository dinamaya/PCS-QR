using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CCIMS.Web.Controllers.API
{
	[Route("api/fetch")]
	[ApiController]
	public class ProvinceFetchController : ControllerBase
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfigurationRepository _configRepo;

		public ProvinceFetchController(IHttpClientFactory httpClientFactory, IConfigurationRepository configRepo)
		{
			_httpClientFactory = httpClientFactory;
			_configRepo = configRepo;
		}


		[HttpGet("city")]
		public async Task<IActionResult> GetCitiesByProvince(string provinceCode)
		{
			var client = _httpClientFactory.CreateClient();
			try
			{
				string url;
				if (provinceCode == "1300000000") // NCR special case
				{
					url = "https://psgc.cloud/api/regions/1300000000/cities-municipalities";
				}
				else if (!string.IsNullOrEmpty(provinceCode)) // Regular province case
				{
					url = _configRepo.GetPSGCCitiesByProvinceCode(provinceCode);
				}
				else
				{
					return BadRequest("No valid province or region code provided.");
				}

				var response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				var result = JsonSerializer.Deserialize<IEnumerable<ProvinceDto>>(json) ?? Enumerable.Empty<ProvinceDto>();
				var filtered = result.Select(r => new AutoSuggestResponseDto()
				{
					Label = r.Name,
					Value = r.Code
				});

				return Ok(filtered);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

		[HttpGet("barangay")]
		public async Task<IActionResult> GetBarangaysByCity(string cityMunCode)
		{
			var client = _httpClientFactory.CreateClient();
			try
			{
				string url = _configRepo.GetPSGCBarangaysByCityCode(cityMunCode);
				var response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				var result = JsonSerializer.Deserialize<IEnumerable<BarangayDto>>(json) ?? Enumerable.Empty<BarangayDto>();
				var filtered = result.Select(r => new AutoSuggestResponseDto()
				{
					Label = r.Name,
					Value = r.Code
				});

				return Ok(filtered);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}
	}
}
