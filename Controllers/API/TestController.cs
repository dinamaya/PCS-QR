using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Services.Interfaces;
using CCIMS.Web.Models.ViewModels;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/test")]
  [ApiController]
  public class TestController : ControllerBase
  {
    private readonly ISecurityRepository _secRepo;
    private readonly IQRRepository _qrRepo;
    private readonly ICaseRepository _caseRepo;
    private readonly IEmailService _emailService;

    public TestController(ISecurityRepository secRepo, IQRRepository qrRepo, ICaseRepository caseRepo, IEmailService emailService)
    {
      _secRepo = secRepo;
      _qrRepo = qrRepo;
      _caseRepo = caseRepo;
      _emailService = emailService;
    }

    [HttpGet("qrLink")]
    public async Task<string> QrLink(string id)
    {
      string _id = await _secRepo.EncryptIDAsync(id);
      return $"https://localhost:8585/Customer/Scan?data={_id}";
    }


    [HttpGet("case/ref/generate")]
    public async Task<string> Case(int x=1)
    {
      string result = string.Empty;

      for (int y=0; y < x; y++)
        result += _caseRepo.GenerateCaseNumber() + "\n";

      return result;
    }

    [HttpGet("case/email/test")]
    public async Task<ActionResult<ResponseDto>> CaseEmailTest(string email, string caseNumber, string sp, string sn, string customerName)
    {
      var response = new ResponseDto();
      try
      {
        await _emailService.TestSendCaseCreationEmailAsync(email, caseNumber, sp, sn, customerName);
        response.Message = "Email Send Successfully";
        return response;
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
        response.Message = ex.Message;
        return response;
      }
    }
  }
}