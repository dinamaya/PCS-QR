using CCIMS.Web.Context;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Repositories.Implementations
{
  public class ServicePartnerRepository : IServicePartnerRepository
  {
    private readonly MainDbContext _mainDb;

    public ServicePartnerRepository(MainDbContext mainDb)
    {
      _mainDb = mainDb;
    }

    public Task<string> GetId()
    {
      throw new NotImplementedException();
    }
  }
}
