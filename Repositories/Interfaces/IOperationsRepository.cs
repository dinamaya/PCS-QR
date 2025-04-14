using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface IOperationsRepository : ICreateRepository<StatusDto>, IEditRepository<StatusEditRequestDto>
  {
    Task<IEnumerable<StatusRowViewModel>> GetAllStatus();
    Task<StatusDto> GetStatusById(string id);
  }
}
