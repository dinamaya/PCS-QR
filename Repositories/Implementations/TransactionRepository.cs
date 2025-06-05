using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using CCIMS.Web.Services.Implementations;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace CCIMS.Web.Repositories.Implementations
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly MainDbContext _mainDb;
		private readonly IOperationsRepository _opsRepo;
		private readonly IConfigurationRepository _configRepo;
		private readonly IEmailService _emailService;
		private readonly ILogger<TransactionRepository> _logger;

    public TransactionRepository(MainDbContext mainDb, IOperationsRepository opsRepo, IEmailService emailService, ILogger<TransactionRepository> logger, IConfigurationRepository configRepo)
    {
      _mainDb = mainDb;
      _opsRepo = opsRepo;
      _emailService = emailService;
      _logger = logger;
      _configRepo = configRepo;
    }

    public string InsertedId { get; set; }

		public async Task CreateAsync(TransactionCreationDto data, string createdBy)
		{
			var date = DateTime.Now.ToLocalTime();

			var transactions = _mainDb.Transactions.Where(s => s.IsActive && s.CaseID == data.CaseId);
			if (transactions.Any())
			{
				await transactions
				.ExecuteUpdateAsync(qr => qr
				  .SetProperty(p => p.IsActive, false)
				  .SetProperty(p => p.DateModified, date)
				  .SetProperty(p => p.ModifiedBy, createdBy)
				);
			}

			Transaction transaction = new()
			{
				CaseID = data.CaseId,
				Comments = data.Comments,
				StatusId = data.StatusId,
				CreatedBy = createdBy,
				DateCreated = date,
				ModifiedBy = createdBy,
				DateModified = date,
				IsActive = true,
			};

			await _mainDb.Transactions.AddAsync(transaction);
			await _mainDb.SaveChangesAsync();

			// Send email notification if case is closed
			try
			{
				var status = await _opsRepo.GetStatusById(data.StatusId);
				var caseDetails = await _mainDb.CaseDetailsVs
					.AsNoTracking()
					.FirstOrDefaultAsync(cd => cd.Id == data.CaseId);


				if (status != null && status.Name.Equals("Closed", StringComparison.OrdinalIgnoreCase))
				{
					var emailDetails = new CustomerEmailDetailsViewModel(_configRepo, caseDetails.CaseNumber)
					{
						Email = caseDetails.Email,
						Fullname = $"{caseDetails.FirstName} {caseDetails.LastName}",
						ServicePartner = caseDetails.SpName,
					};

					try
					{
						await _emailService.SendCaseClosedNotificationAsync(emailDetails);
						_logger.LogInformation($"Successfully sent case closed email for Case ID: {data.CaseId}, CaseNumber: {caseDetails.CaseNumber}");
					}
					catch (Exception emailEx)
					{
						_logger.LogError(emailEx, $"Failed to send case closed email for Case ID: {data.CaseId}, CaseNumber: {caseDetails.CaseNumber}. Error: {emailEx.Message}");
					}
				}
			}
			catch (Exception ex)
			{
				// Log the error but do not let it break the main transaction flow.
				_logger.LogError(ex, $"Error during email sending logic for case closure, Case ID: {data.CaseId}. Error: {ex.Message}");
			}

			InsertedId = transaction.Id.ToString();
		}

		public async Task<IEnumerable<CaseTransactionsViewModel>> GetAllByCaseId(long caseId)
		{
			return await _mainDb.TransactionsVs
			  .Where(t => t.CaseId == caseId)
			  .Select(t => new CaseTransactionsViewModel()
			  {
				  StatusName = t.Status,
				  Comments = t.Comments,
				  TransactionId = t.Id.ToString(),
				  TransactionDate = t.DateCreated.ToString(Database.DateFormat.DISPLAY_COMPLETE),
				  Icon = "",
				  IsCommentable = t.IsCommentable
			  })
			  .ToListAsync();
		}

		public async Task<IEnumerable<DropdownOptionViewModel>> GetExistingStatusByCaseId(long caseId)
		{
			return await _mainDb.TransactionsVs
			  .Where(t => t.CaseId == caseId)
			  .Select(t => new DropdownOptionViewModel()
			  {
				  Label = t.Status,
				  Value = t.StatusId
			  })
			  .ToListAsync();
		}

		public async Task<IEnumerable<DropdownOptionViewModel>> GetAvailableStatusByCaseId(long caseId)
		{
			var existingStats = await GetExistingStatusByCaseId(caseId);
			var allStats = await _opsRepo.GetOptions();
			var availStats = allStats.Where(s => !existingStats.Any(x => x.Value == s.Value))
			  .Select(s => new DropdownOptionViewModel()
			  {
				  Label = s.Label,
				  Value = s.Value
			  })
			  .ToList();

			return availStats;
		}

		public async Task<TransactionEditResponseDto> GetById(long id)
		{
			var transaction = await _mainDb
			  .TransactionsVs
			  .AsNoTracking()
			  .Where(t => t.Id == id)
			  .Select(t => new TransactionEditResponseDto()
			  {
				  IsCommentable = t.IsCommentable,
				  Remarks = t.Comments
			  })
			  .FirstOrDefaultAsync();

			if (!transaction.IsCommentable)
				throw new Exception(Exceptions.Message.INVALID_TRANSACTION_UNCOMMENTABLE);

			return transaction;
		}

		public async Task EditAsync(TransactionEditRequestDto editRequestDto, string modifiedBy)
		{
			var data = (await _mainDb.Transactions.FindAsync(long.Parse(editRequestDto.Id))) ?? throw new Exception(Exceptions.Message.INVALID_TRANSACTION);
			var stat = await _opsRepo.GetStatusById(data.StatusId);

			if (!stat.IsCommentable) throw new Exception(Exceptions.Message.INVALID_STATUS);

			data.Comments = editRequestDto.Remarks;

			await _mainDb.SaveChangesAsync();
		}
	}
}
