using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface IOperationsRepository : ICreateRepository<StatusCreationRequestDto>
  {
    Task<IEnumerable<StatusRowViewModel>> GetAllStatus();
  }
}
