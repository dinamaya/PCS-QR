using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/test")]
  [ApiController]
  public class TestController : ControllerBase
  {
    private readonly ISecurityRepository _secRepo;
    private readonly IQRRepository _qrRepo;

    public TestController(ISecurityRepository secRepo, IQRRepository qrRepo)
    {
      _secRepo = secRepo;
      _qrRepo = qrRepo;
    }

    [HttpGet("qrLink")]
    public async Task<string> QrLink(string id)
    {
      string _id = await _secRepo.EncryptIDAsync(id);
      return $"https://localhost:34881/Customer/Scan?data={_id}";
    }


    [HttpPost("qrLink")]
    public async Task<string> QrLink([FromBody] QRCreationRequestDto creationDto)
    {
      try
      {
        var date = DateTime.Now;

        await _qrRepo.CreateAsync(new QRCode()
        {
          ServicePartnerId = creationDto.ServicePartnerId,
          Description = creationDto.Description,
          DateCreated = date,
          DateModified = date,
          IsActive = true,
        });

        return "Successfully Created: ";
      }
      catch (Exception ex) { 
        return "Error: " + ex.Message;
      }
    }
  }
}
