using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ITransactionRepository : ICreateRepository<TransactionCreationDto>
  {
    Task<IEnumerable<CaseTransactionsViewModel>> GetAllByCaseId(long caseId);
  }
}
