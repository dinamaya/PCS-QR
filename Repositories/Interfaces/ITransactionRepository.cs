using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface ITransactionRepository : ICreateRepository<TransactionCreationDto>, IEditRepository<TransactionEditRequestDto>
  {
    Task<IEnumerable<CaseTransactionsViewModel>> GetAllByCaseId(long caseId);
    Task<IEnumerable<DropdownOptionViewModel>> GetExistingStatusByCaseId(long caseId);
    Task<IEnumerable<DropdownOptionViewModel>> GetAvailableStatusByCaseId(long caseId);
    Task<TransactionEditResponseDto> GetById(long id);
  }
}
