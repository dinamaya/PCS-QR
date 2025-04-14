using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Repositories.Interfaces.CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CCIMS.Web.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MainDbContext _context;
        private readonly ITokenProvider _tokenProvider;
        private readonly ICaseRepository _caseRepo;
        private readonly ISecurityRepository _securityRepo;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            MainDbContext context,
            ITokenProvider tokenProvider,
            ICaseRepository caseRepo,
            ISecurityRepository securityRepo,
            ILogger<CustomerRepository> logger)
        {
            _context = context;
            _tokenProvider = tokenProvider;
            _caseRepo = caseRepo;
            _securityRepo = securityRepo;
            _logger = logger;
        }

        public async Task<bool> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
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
                    Email = createCustomerDto.Email,
                    DateCreated = DateTime.UtcNow.ToLocalTime(),
                    DateModified = DateTime.UtcNow.ToLocalTime(),
                    IsActive = true,
                    ModifiedBy = string.Empty
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                var newCase = new Case
                {
                    CustomerID = customer.Id,
                    QRCodeId = await _securityRepo.DecryptIDAsync(token.QRID),
                    SerialNumber = createCustomerDto.SerialNumber
                };

                await _caseRepo.CreateCaseAsync(newCase);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateCustomerAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CustomerExistsAsync(string email)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email && c.IsActive);
        }
    }
}