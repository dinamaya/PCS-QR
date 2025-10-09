using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Implementations;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Implementations;
using CCIMS.Web.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
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
		private readonly ISecurityRepository _secRepo;
        private readonly IRatingRepository _ratingRepo;
        private readonly MainDbContext _mainDb;

    public CustomerController(
      ICustomerRepository customerRepository,
      ILogger<CustomerController> logger, MainDbContext mainDb, ISecurityRepository securityRepo, IConfigurationRepository configRepo, ITokenProvider tokenProvider, IServicePartnerRepository spRepo, IEmailService emailService, ISecurityRepository secRepo, IRatingRepository ratingRepo)
    {
      _ratingRepo = ratingRepo;
      _customerRepository = customerRepository;
      _logger = logger;
      _configRepo = configRepo;
      _tokenProvider = tokenProvider;
      _mainDb = mainDb;
      _securityRepo = securityRepo;
      _spRepo = spRepo;
      _emailService = emailService;
      _secRepo = secRepo;
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
				QRTokenDto qrToken = null;
				_tokenProvider.IsValid(createCustomerDto.Token, out qrToken);

				string decryptedQrId = await _secRepo.DecryptIDAsync(qrToken.QRID);
                string email = await _mainDb.ServicePartnersVs.AsNoTracking().Where(s => s.QrId == decryptedQrId).Select(s => s.Email).FirstOrDefaultAsync();
                string caseNumber = await _customerRepository.CreateCustomerCaseAsync(createCustomerDto);
				string encCaseNumber = await _secRepo.EncryptIDAsync(caseNumber);

        BackgroundJob.Enqueue<BackgroundJobsService>(
          (service) => service.SendCaseCreateEmail(createCustomerDto, caseNumber, encCaseNumber, email));

				_tokenProvider.Remove(createCustomerDto.Token);
				ViewData[Keys.ViewData.CASE] = caseNumber; 

        return View("ThankYou", encCaseNumber);
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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Feedback(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is missing.");
                }

                string caseNumber = await _secRepo.DecryptIDAsync(token);
                if (string.IsNullOrEmpty(caseNumber))
                {
                    throw new Exception("Invalid token.");
                }

                var model = new FeedbackViewModel
                {
                    Token = token,
                    CaseNumber = caseNumber
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
        public async Task<IActionResult> Feedback(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please fill in the required fields";
                return View(model);
            }

            try
            {
                string caseNumber = await _secRepo.DecryptIDAsync(model.Token);
                var caseEntity = await _mainDb.Cases.FirstOrDefaultAsync(c => c.CaseNumber == caseNumber);
                if (caseEntity == null)
                {
                    throw new Exception("Invalid case number.");
                }

                var existingRating = await _mainDb.Ratings.FirstOrDefaultAsync(r => r.CaseId == caseEntity.Id);
                if (existingRating != null)
                {
                    ViewBag.ErrorMessage = "Feedback has already been submitted.";
                    return View(model);
                }

                var rating = new Models.Entities.Main.Rating
                {
                    CaseId = caseEntity.Id,
                    CaseNumber = caseNumber,
                    CustomerId = caseEntity.CustomerID,
                    RatingVal = model.Rating,
                    Comment = model.Comment,
                    DateCreated = DateTime.UtcNow
                };

                await _ratingRepo.AddRatingAsync(rating);

                ViewBag.IsFeedback = true;
                return View("ThankYou");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error submitting feedback: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }
    }
}