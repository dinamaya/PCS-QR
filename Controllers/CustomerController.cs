using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CCIMS.Web.Controllers
{
    public partial class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            ICustomerRepository customerRepository,
            ILogger<CustomerController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register(string token)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateCustomerDto createCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid form submission.";
                return View(createCustomerDto.Token);
            }

            try
            {
                await _customerRepository.CreateCustomerAsync(createCustomerDto);
                return View("ThankYou");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving customer: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View(createCustomerDto.Token);
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