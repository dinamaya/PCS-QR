using CCIMS.Web.Context;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;

namespace CCIMS.Web.Repositories.Interfaces
{
  public interface IQRRepository : ICreateRepository<QRCode>
  {
    Task<byte[]> GetById(string qrId);
	Task<bool> QRCodeExistsAsync(string qrId);
	}
}
