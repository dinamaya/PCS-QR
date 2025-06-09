using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Implementations;
using CCIMS.Web.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CCIMS.Web.Controllers
{
	public partial class CustomerController : Controller
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly ILogger<CustomerController> _logger;
		private readonly IConfigurationRepository _configRepo;
		private readonly ITokenProvider _tokenProvider;
		private readonly ISecurityRepository _securityRepo;
		private readonly IServicePartnerRepository _spRepo;
		private readonly IEmailService _emailService;
		private readonly MainDbContext _mainDb;

		public CustomerController(
			ICustomerRepository customerRepository,
			ILogger<CustomerController> logger, MainDbContext mainDb, ISecurityRepository securityRepo, IConfigurationRepository configRepo, ITokenProvider tokenProvider, IServicePartnerRepository spRepo, IEmailService emailService)
		{
			_customerRepository = customerRepository;
			_logger = logger;
			_configRepo = configRepo;
			_tokenProvider = tokenProvider;
			_mainDb = mainDb;
			_securityRepo = securityRepo;
			_spRepo = spRepo;
			_emailService = emailService;
		}

		[HttpGet]
		public async Task<IActionResult> Register(string token)
		{
			try
			{
				QRTokenDto? qrToken = null;
				if (!_tokenProvider.IsValid(token, out qrToken) || qrToken == null)
					throw new Exception(Exceptions.Message.INVALID_QRTOKEN);

				string origQrId = await _securityRepo.DecryptIDAsync(qrToken!.QRID);

				bool doesExist = await _mainDb.QRCodes.AnyAsync(q => q.Id == origQrId);
				if (!doesExist)
					throw new Exception(Exceptions.Message.INVALID_QRREFERENCE);

				ViewData[Keys.ViewData.SPNAME] = await _spRepo.GetNameByQrId(origQrId);

				var model = new CreateCustomerDto
				{
					Token = token,
					ServicePartner = ViewData[Keys.ViewData.SPNAME]?.ToString()
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
				if (!ModelState.IsValid)
				{
					ViewBag.ErrorMessage = "Please fill in the required fields";
					return View("Register", createCustomerDto.Token);
				}

				string caseNumber = await _customerRepository.CreateCustomerCaseAsync(createCustomerDto);

        BackgroundJob.Enqueue<BackgroundJobsService>(
          (service) => service.SendCaseCreateEmail(createCustomerDto, caseNumber));

				_tokenProvider.Remove(createCustomerDto.Token);

				return View("ThankYou", caseNumber);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error saving customer: {ex.Message}");
				ViewBag.ErrorMessage = ex.Message;
				return View("Register", createCustomerDto);
			}
		}

		[HttpGet]
		public IActionResult Scan(string data)
		{
			try
			{
				if (string.IsNullOrEmpty(data))
					throw new Exception("Scan Data is Empty");

				return View(model: data);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error: {ex.Message}");
				return RedirectToAction("Index", "Home");
			}
		}
	}
}