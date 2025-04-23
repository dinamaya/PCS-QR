using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;
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
        private readonly MainDbContext _mainDb;

        public CustomerController(
            ICustomerRepository customerRepository,
            ILogger<CustomerController> logger, MainDbContext mainDb, ISecurityRepository securityRepo, IConfigurationRepository configRepo, ITokenProvider tokenProvider)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _configRepo = configRepo;
            _tokenProvider = tokenProvider;
            _mainDb = mainDb;
            _securityRepo = securityRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Register(string token)
        {

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

				TempData["Token"] = token;
				var model = new CreateCustomerDto { Token = token };

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

			createCustomerDto.Token = createCustomerDto.Token ?? TempData["Token"]?.ToString();

			if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid form submission.";
                return View(createCustomerDto.Token);
            }

            try
            {
                await _customerRepository.CreateCustomerCaseAsync(createCustomerDto);
                return View("ThankYou");
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