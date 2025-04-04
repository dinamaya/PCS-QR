using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using main =  CCIMS.Web.Models.Entities.Main;

using CCIMS.Web.Repositories.Interfaces;
using QRCoder;
using CCIMS.Web.App_Code._Globals.Constants;
using Microsoft.AspNetCore.Http;

namespace CCIMS.Web.Repositories.Implementations
{
  public class QRRepository : IQRRepository
  {
    private readonly MainDbContext _mainDb;
    private readonly ISecurityRepository _secRepo;
    private readonly IConfigurationRepository _configRepo;

    public QRRepository(MainDbContext mainDb, ISecurityRepository secRepo, IConfigurationRepository configRepo)
    {
      _mainDb = mainDb;
      _secRepo = secRepo;
      _configRepo = configRepo;
    }

    public string InsertedId { get; set ; }

    public async Task CreateAsync(main.QRCode data)
    {
      await _mainDb.QRCodes.AddAsync(data);
      await _mainDb.SaveChangesAsync();
      InsertedId = data.Id;
		}

    public async Task<byte[]> GetById(string qrId)
    {
      string _id = await _secRepo.EncryptIDAsync(qrId);
      
      var data = await _mainDb.QRCodes.FindAsync(qrId) ?? throw new InvalidOperationException(Exceptions.Message.INVALID_QRREFERENCE);
      var content = $"{_configRepo.GetQrScanUrl()}?data={_id}";
      using QRCodeGenerator qrGenerator = new QRCodeGenerator();
      QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
      PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

      return qrCode.GetGraphic(20);
    }
  }
}
