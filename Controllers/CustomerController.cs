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
using System.Net.Mail;

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
				QRTokenDto? qrToken = null;
				if (!_tokenProvider.IsValidToken(token, out qrToken))
					throw new Exception(Exceptions.Message.INVALID_QRTOKEN);

				if (qrToken == null)
					throw new Exception(Exceptions.Message.INVALID_QRTOKEN);

				string origQrId = await _securityRepo.DecryptIDAsync(qrToken!.QRID);
				bool doesExist = await _mainDb.QRCodes.AnyAsync(q => q.Id == origQrId);
				if (!doesExist)
					throw new Exception(Exceptions.Message.INVALID_QRREFERENCE);

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
				provincesList.Insert(0, ncrProvince);

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
		public async Task<IActionResult> Register(CreateCustomerDto createCustomerDto)
		{
			try
			{
				var customer = new Customer();
				var newCase = new Case();
				QRTokenDto token = null;
				bool isTokenValid = _tokenProvider.IsValidToken(createCustomerDto.Token, out token);

				if (!isTokenValid && token != null) throw new Exception("Invalid Token");

				customer.FirstName = createCustomerDto.FirstName;
				customer.LastName = createCustomerDto.LastName;
				customer.Address1 = createCustomerDto.Address1;
				customer.Address2 = createCustomerDto.Address2;
				customer.Province = createCustomerDto.Province;
				customer.CityMunicipality = createCustomerDto.CityMunicipality;
				customer.Barangay = createCustomerDto.Barangay;
				customer.ContactNumber = createCustomerDto.ContactNumber;
				customer.Email = createCustomerDto.Email;

				customer.DateCreated = DateTime.UtcNow;
				customer.DateModified = DateTime.UtcNow;
				customer.IsActive = true;
				customer.ModifiedBy = string.Empty;

				_mainDb.Customers.Add(customer);
				await _mainDb.SaveChangesAsync();

				newCase.CaseNumber = Guid.NewGuid().ToString();
				newCase.CustomerID = customer.Id;
				newCase.Description = string.Empty;
				newCase.QRCodeId = await _securityRepo.DecryptIDAsync(token.QRID);
				newCase.SerialNumber = createCustomerDto.SerialNumber;
				newCase.ModifiedBy = string.Empty;
				newCase.DateCreated = DateTime.Now; 
				newCase.IsActive = true;

				_mainDb.Cases.Add(newCase);
				await _mainDb.SaveChangesAsync();

				// Redirect after successful submission
				return View("ThankYou");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error saving customer: {ex.Message}");
				ViewBag.ErrorMessage = "There was an error processing your request.";

				// Need to reload provinces for the view
				var client = _httpClientFactory.CreateClient();
				client.Timeout = TimeSpan.FromSeconds(30);

				try
				{
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
					provincesList.Insert(0, ncrProvince);

					var model = new ProvincesViewModel
					{
						Provinces = provincesList
					};

					return View(model);
				}
				catch
				{
					return RedirectToAction("Index", "Home");
				}
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
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Home");
			}
		}
	}
}