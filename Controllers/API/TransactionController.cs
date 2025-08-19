using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/transaction")]
  [ApiController, Authorize(Roles = "SPA")]
  public class TransactionController : ControllerBase
  {
    private readonly ITransactionRepository _transRepo;

    public TransactionController(ITransactionRepository transRepo)
    {
      _transRepo = transRepo;
    }


    [HttpGet]
    public async Task<ActionResult<ResponseDto<TransactionEditResponseDto>>> Get([FromQuery] string id)
    {
      var response = new ResponseDto<TransactionEditResponseDto>();
      try
      {
        var result = await _transRepo.GetById(long.Parse(id));

        response.Result = result;
        response.Message = "Transaction (" + id + ") found";

        return Ok(response);
      }
      catch (Exception ex)
      {
        response.Message = "Error: " + ex.Message;
        response.IsSuccess = false;

        return BadRequest(response);
      }
    }

    [HttpPut]
    public async Task<ActionResult<ResponseDto>> Put([FromBody] TransactionEditRequestDto requestDto)
    {
      var response = new ResponseDto();
      try
      {
        string modifiedBy = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _transRepo.EditAsync(requestDto, modifiedBy);

        response.Message = "Transaction (" + requestDto.Id + ") Updated";

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
