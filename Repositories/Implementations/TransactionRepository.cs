using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CCIMS.Web.Repositories.Implementations
{
  public class TransactionRepository : ITransactionRepository
  {
    private readonly MainDbContext _mainDb;

    public TransactionRepository(MainDbContext mainDb)
    {
      _mainDb = mainDb;
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
        Comments =  data.Comments,
        StatusId = data.StatusId,
        CreatedBy = createdBy,
        DateCreated = date,
        ModifiedBy = createdBy,
        DateModified = date,
        IsActive = true,
      };

      await _mainDb.Transactions.AddAsync(transaction);
      await _mainDb.SaveChangesAsync();

      InsertedId = transaction.Id.ToString();
    }

  }
}
