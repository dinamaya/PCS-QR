using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Repositories.Interfaces.CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CCIMS.Web.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfigurationRepository _configRepo;
		private readonly ITokenProvider _tokenProvider;
		private readonly ILogger<CustomerService> _logger;
		private readonly ICustomerRepository _customerRepo;
		private readonly ICaseRepository _caseRepo;
		private readonly IQRRepository _qrCodeRepo;
		private readonly ISecurityRepository _securityRepo;

		public CustomerService(
			IHttpClientFactory httpClientFactory,
			IConfigurationRepository configRepo,
			ITokenProvider tokenProvider,
			ILogger<CustomerService> logger,
			ICustomerRepository customerRepo,
			ICaseRepository caseRepo,
			IQRRepository qrCodeRepo,
			ISecurityRepository securityRepo)
		{
			_httpClientFactory = httpClientFactory;
			_configRepo = configRepo;
			_tokenProvider = tokenProvider;
			_logger = logger;
			_customerRepo = customerRepo;
			_caseRepo = caseRepo;
			_qrCodeRepo = qrCodeRepo;
			_securityRepo = securityRepo;
		}

		public async Task<ProvincesViewModel> GetRegisterViewModelAsync(string token)
		{
			var client = _httpClientFactory.CreateClient();
			client.Timeout = TimeSpan.FromSeconds(30);

			try
			{
				if (!_tokenProvider.IsValidToken(token, out QRTokenDto? qrToken) || qrToken == null)
					throw new Exception("Invalid QR token");

				string origQrId = await _securityRepo.DecryptIDAsync(qrToken.QRID);
				if (!await _qrCodeRepo.QRCodeExistsAsync(origQrId))
					throw new Exception("Invalid QR reference");

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

				return new ProvincesViewModel
				{
					Provinces = provincesList
				};
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in GetRegisterViewModelAsync: {ex.Message}");
				throw;
			}
		}

		public async Task<bool> RegisterCustomerAsync(CreateCustomerDto createCustomerDto)
		{
			try
			{
				if (!_tokenProvider.IsValidToken(createCustomerDto.Token, out QRTokenDto? token) || token == null)
					throw new Exception("Invalid Token");

				var customer = new Customer
				{
					FirstName = createCustomerDto.FirstName,
					LastName = createCustomerDto.LastName,
					Address = createCustomerDto.Address,
					ContactNumber = createCustomerDto.ContactNumber,
					Email = createCustomerDto.Email
				};

				var createdCustomer = await _customerRepo.CreateCustomerAsync(customer);

				var newCase = new Case
				{
					CustomerID = createdCustomer.Id,
					QRCodeId = await _securityRepo.DecryptIDAsync(token.QRID),
					SerialNumber = createCustomerDto.SerialNumber
				};

				await _caseRepo.CreateCaseAsync(newCase);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in RegisterCustomerAsync: {ex.Message}");
				throw;
			}
		}
	}
}
