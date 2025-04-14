using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Repositories.Implementations
{
  public class CaseRepository : ICaseRepository
  {
    public Task<IEnumerable<CaseRowViewModel>> GetAll()
    {
      throw new NotImplementedException();
    }

    public Task<CaseRowViewModel> GetById(string id)
    {
      throw new NotImplementedException();
    }
  }
}
