using CCIMS.Web.Context;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Repositories.Implementations
{
  public class QRRepository : IQRRepository
  {
    private readonly MainDbContext _mainDb;

    public QRRepository(MainDbContext mainDb)
    {
      _mainDb = mainDb;
    }

    public string InsertedId { get; set ; }

    public async Task CreateAsync(QRCode data)
    {
      await _mainDb.QRCodes.AddAsync(data);
      await _mainDb.SaveChangesAsync();
    }
  }
}
