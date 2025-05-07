using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace CCIMS.Web.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MainDbContext _context;
        private readonly ITokenProvider _tokenProvider;
        private readonly ICaseRepository _caseRepo;
        private readonly ISecurityRepository _securityRepo;
        private readonly ITransactionRepository _transactionRepo;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            MainDbContext context,
            ITokenProvider tokenProvider,
            ICaseRepository caseRepo,
            ISecurityRepository securityRepo,
            ILogger<CustomerRepository> logger,
            ITransactionRepository transactionRepo)
        {
            _context = context;
            _tokenProvider = tokenProvider;
            _caseRepo = caseRepo;
            _securityRepo = securityRepo;
            _transactionRepo = transactionRepo;
            _logger = logger;
        }

        public async Task<string> CreateCustomerCaseAsync(CreateCustomerDto createCustomerDto)
        {
            if (!_tokenProvider.IsValidToken(createCustomerDto.Token, out QRTokenDto? token) || token == null)
                throw new Exception("Invalid Token");

			var existingCase = await _context.LatestCasesVs
	            .Where(c => c.SerialNumber == createCustomerDto.SerialNumber && c.Status != "Closed")
	            .Select(c => new { c.CaseNumber })
	            .FirstOrDefaultAsync();

			if (existingCase != null)
				throw new Exception($"Serial number already exists with Case Number: {existingCase.CaseNumber}. Please provide a unique serial number.");

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

            await _caseRepo.CreateAsync(newCase, "");
            var statId = (await _context.Statuses.Where(s => s.Name == "On-Queue").FirstOrDefaultAsync()).Id;

            var transaction = new TransactionCreationDto()
            {
                CaseId = newCase.Id,
                Comments = "",
                StatusId = statId
            };

            await _transactionRepo.CreateAsync(transaction, "");

            return newCase.CaseNumber; // Return the case number
        }

        public async Task<bool> CustomerExistsAsync(string email) => await _context.Customers.AnyAsync(c => c.Email == email && c.IsActive);

        public async Task<CustomerDetailsViewModel> GetById(string id)
        {
            return await _context.Customers
              .Where(c => c.IsActive && c.Id == id)
              .Select(c => new CustomerDetailsViewModel()
              {
                  Id = c.Id,
                  Firstname = c.FirstName,
                  Lastname = c.LastName,
                  Email = c.Email,
                  ContactNo = c.ContactNumber,
                  Address = c.Address,
              })
              .FirstOrDefaultAsync() ??
              throw new Exception(Exceptions.Message.INVALID_CATEGORY);
        }
    }
}