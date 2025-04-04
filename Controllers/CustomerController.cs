using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.Models.Entities.Main;

namespace CCIMS.Web.Controllers
{
	public partial class CustomerController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfigurationRepository _configRepo;
		private readonly ITokenProvider _tokenProvider;
		private readonly ILogger<CustomerController> _logger;
		private readonly ISecurityRepository _securityRepo;
		private readonly MainDbContext _mainDb;

		public CustomerController(IHttpClientFactory httpClientFactory, IConfigurationRepository configRepo, ITokenProvider tokenProvider, ILogger<CustomerController> logger, MainDbContext mainDb, ISecurityRepository securityRepo)
		{
			_httpClientFactory = httpClientFactory;
			_configRepo = configRepo;
			_tokenProvider = tokenProvider;
			_logger = logger;
			_mainDb = mainDb;
			_securityRepo = securityRepo;
		}

		[HttpGet]
		public async Task<IActionResult> Register(string token)
		{
			var client = _httpClientFactory.CreateClient();
			client.Timeout = TimeSpan.FromSeconds(30);

			try
			{
				//QRTokenDto? qrToken = null;
				//if (!_tokenProvider.IsValidToken(token, out qrToken))
				//	throw new Exception(Exceptions.Message.INVALID_QRTOKEN);

				//if(qrToken == null)
				//	throw new Exception(Exceptions.Message.INVALID_QRTOKEN);

				//string origQrId = await _securityRepo.DecryptIDAsync(qrToken!.QRID);
				//bool doesExist = await _mainDb.QRCodes.AnyAsync(q => q.Id == origQrId);
				//if (!doesExist)
				//	throw new Exception(Exceptions.Message.INVALID_QRREFERENCE);

				var provinceResponse = await client.GetAsync(_configRepo.GetPSGCProvinces());
				provinceResponse.EnsureSuccessStatusCode();
				var provinceJson = await provinceResponse.Content.ReadAsStringAsync();
				var provinces = JsonSerializer.Deserialize<IEnumerable<ProvinceDto>>(provinceJson) ?? Enumerable.Empty<ProvinceDto>();

				
				var ncrProvince = new ProvinceDto
				{
					Id = 0, 
					Name = "National Capital Region (NCR)",
					Code = "1300000000", 
					RegionId = 13 
				};

				
				var provincesList = provinces.ToList();
				provincesList.Add(ncrProvince);

				var model = new ProvincesViewModel
				{
					Provinces = provincesList
				};

				return View(model);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error: {ex.Message}");
				ViewBag.ErrorMessage = ex.Message;
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegistrationViewModel model, string token)
		{
			var client = _httpClientFactory.CreateClient();
			client.Timeout = TimeSpan.FromSeconds(30);

			try
			{
				if (!ModelState.IsValid)
				{
					// Re-fetch provinces for the view
					var provinceResponse = await client.GetAsync(_configRepo.GetPSGCProvinces());
					provinceResponse.EnsureSuccessStatusCode();
					var provinceJson = await provinceResponse.Content.ReadAsStringAsync();
					model.Provinces = JsonSerializer.Deserialize<IEnumerable<ProvinceDto>>(provinceJson) ?? Enumerable.Empty<ProvinceDto>();
					return View(model);
				}

				// Example: Save to database (adjust based on your entity model)
				var customer = new Customer
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					ContactNumber = model.ContactNumber,
					Email = model.Email,
					Address1 = model.Address1,
					Address2 = model.Address2,
					Province = model.Province,
					CityMunicipality = model.CityMunicipality,
					Barangay = model.Barangay,
					SerialNumber = model.SerialNumber,
					DateCreated = DateTime.UtcNow
				};

				_mainDb.Customers.Add(customer);
				await _mainDb.SaveChangesAsync();

				// Redirect to a success page or home
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error saving customer data: {ex.Message}");
				ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again.";

				// Re-fetch provinces for the view
				var provinceResponse = await client.GetAsync(_configRepo.GetPSGCProvinces());
				provinceResponse.EnsureSuccessStatusCode();
				var provinceJson = await provinceResponse.Content.ReadAsStringAsync();
				model.Provinces = JsonSerializer.Deserialize<IEnumerable<ProvinceDto>>(provinceJson) ?? Enumerable.Empty<ProvinceDto>();

				return View(model);
			}
		}

		[HttpGet]
		public IActionResult Scan(string data)
		{
			try
			{
				if (data.IsNullOrEmpty()) throw new Exception("Scan Data is Empty");

				return View(model: data);
			}
			catch(Exception ex)
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		public async Task<IActionResult> FetchCities(string provinceCode)
		{
			var client = _httpClientFactory.CreateClient();
			client.Timeout = TimeSpan.FromSeconds(30);

			try
			{
				string requestUrl;

				// Special case for NCR (Region 13, code 1300000000)
				if (provinceCode == "1300000000")
				{
					requestUrl = _configRepo.GetPSGCCitiesByNCRRegion(provinceCode);
				}
				else
				{
					requestUrl = _configRepo.GetPSGCCitiesByProvinceCode(provinceCode);
				}

				var response = await client.GetAsync(requestUrl);
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var cities = JsonSerializer.Deserialize<IEnumerable<CityDto>>(json) ?? Enumerable.Empty<CityDto>();

				return Json(cities);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error fetching cities: {ex.Message}");
				return StatusCode(500, "Error fetching cities");
			}
		}

	}
}

