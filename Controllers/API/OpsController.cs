using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/ops")]
  [ApiController]
  public class OpsController : ControllerBase
  {
    private readonly IOperationsRepository _opsRepo;

    public OpsController(IOperationsRepository opsRepo)
    {
      _opsRepo = opsRepo;
    }

    [HttpPost("status"), Authorize]
    public async Task<ActionResult<ResponseDto>> Status([FromBody] StatusCreationRequestDto creationRequest)
    {
      var response = new ResponseDto();
      try
      {
        var date = DateTime.Now;
        string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _opsRepo.CreateAsync(creationRequest, accountId);

        response.Message = "Status created successfully";

        return Ok(response);
      }
      catch (Exception ex)
      {
        response.Message = "Error: " + ex.Message;
        response.IsSuccess = false;

        return BadRequest(response);
      }
    }
  }
}
