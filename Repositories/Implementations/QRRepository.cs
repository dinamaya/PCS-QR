using CCIMS.Web.Context;
using main =  CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using QRCoder;
using CCIMS.Web.App_Code._Globals.Constants;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using CCIMS.Web.App_Code._Globals;

namespace CCIMS.Web.Repositories.Implementations
{
  public class QRRepository : IQRRepository
  {
    private readonly Server _server;
    private readonly MainDbContext _mainDb;
    private readonly ISecurityRepository _secRepo;
    private readonly IConfigurationRepository _configRepo;

    public QRRepository(MainDbContext mainDb, ISecurityRepository secRepo, IConfigurationRepository configRepo, Server server)
    {
      _mainDb = mainDb;
      _secRepo = secRepo;
      _configRepo = configRepo;
      _server = server;
    }

    public string InsertedId { get; set ; }

    public async Task CreateAsync(main.QRCode data, string createdBy)
    {
      await DeactivateActiveQRs(data.ServicePartnerId);

      await _mainDb.QRCodes.AddAsync(data);
      await _mainDb.SaveChangesAsync();
      InsertedId = data.Id;
		}

    public async Task<byte[]> GetById(string qrId)
    {
      var data = await _mainDb.QRCodes.FindAsync(qrId) ?? throw new InvalidOperationException(Exceptions.Message.INVALID_QRREFERENCE2);
      
      string _id = await _secRepo.EncryptIDAsync(qrId);
      var content = $"{_configRepo.GetQrScanUrl()}?data={_id}";

      return GenerateQr(content);
    }

    private async Task DeactivateActiveQRs(string spId)
    {
      var now = DateTime.Now;

      await _mainDb.QRCodes
          .Where(s => s.IsActive && s.ServicePartnerId == spId)
          .ExecuteUpdateAsync(qr => qr
              .SetProperty(p => p.IsActive, false)
              .SetProperty(p => p.DateModified, now)
          );
    }

	  public async Task<bool> QRCodeExistsAsync(string qrId)
	  {
		  return await _mainDb.QRCodes.AnyAsync(q => q.Id == qrId);
	  }

    private byte[] GenerateQr(string content)
    {
      string iconPath = Path.Combine(_server.RootDirectory, "img", "icons", "icon_vst.png");
      Bitmap iconBitmap = new Bitmap(iconPath);

      using QRCodeGenerator qrGenerator = new QRCodeGenerator();
      QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
      using QRCode qrCode = new QRCode(qrCodeData);

      Bitmap qrCodeImage = qrCode.GetGraphic(
          pixelsPerModule: 20,
          darkColor: Color.Black,
          lightColor: Color.White,
          icon: iconBitmap,
          iconSizePercent: 20,
          iconBorderWidth: 1,
          drawQuietZones: true);

      using MemoryStream ms = new MemoryStream();
      qrCodeImage.Save(ms, ImageFormat.Png);

      return ms.ToArray();
    }
  }
}
