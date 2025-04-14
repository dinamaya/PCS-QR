using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CCIMS.Web.Controllers
{
	public partial class CustomerController : Controller
	{
		private readonly ICustomerService _customerService;
		private readonly ILogger<CustomerController> _logger;

		public CustomerController(
			ICustomerService customerService,
			ILogger<CustomerController> logger)
		{
			_customerService = customerService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Register(string token)
		{
			try
			{
				var model = await _customerService.GetRegisterViewModelAsync(token);
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
			if (!ModelState.IsValid)
			{
				ViewBag.ErrorMessage = "Invalid form submission.";
				return await ReloadProvincesView(createCustomerDto.Token);
			}

			try
			{
				await _customerService.RegisterCustomerAsync(createCustomerDto);
				return View("ThankYou");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error saving customer: {ex.Message}");
				ViewBag.ErrorMessage = ex.Message;
				return await ReloadProvincesView(createCustomerDto.Token);
			}
		}

		private async Task<IActionResult> ReloadProvincesView(string token)
		{
			try
			{
				var model = await _customerService.GetRegisterViewModelAsync(token);
				return View(model);
			}
			catch
			{
				return RedirectToAction("Index", "Home");
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