using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/cases")]
  [ApiController]
  public class CasesController : ControllerBase
  {
    private readonly ICaseRepository _caseRepo;
    private readonly ITransactionRepository _transactionRepo;

    public CasesController(ICaseRepository caseRepo, ITransactionRepository transactionRepo)
    {
      _caseRepo = caseRepo;
      _transactionRepo = transactionRepo;
    }

    [HttpGet("status")]
    public async Task<ActionResult<ResponseDto<CaseStatusDto>>> Status(long caseId)
    {
      var response = new ResponseDto<CaseStatusDto>();
      try
      {
        var currStat = await _caseRepo.GetCurrentStatus(caseId);
        var availStats = await _transactionRepo.GetAvailableStatusByCaseId(caseId);

        response.Message = "Status fetched";
        response.Result = new()
        {
          CurrentStatus = currStat,
          AvailableStatus = DropdownOptionViewModel.InitOptions("Choose new status", availStats)
        };

        return Ok(response);
      }
      catch (Exception ex)
      {
        response.Message = "Error: " + ex.Message;
        response.IsSuccess = false;

        return BadRequest(response);
      }
    }

    [HttpPut("status")]
    public async Task<ActionResult<ResponseDto>> Status([FromBody] CaseStatusUpdateRequestDto caseStatusUpdateRequest)
    {
      var response = new ResponseDto();
      try
      {
        string createdBy = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _transactionRepo.CreateAsync(caseStatusUpdateRequest, createdBy);

        response.Message = "Status updated";

        return Ok(response);
      }
      catch (Exception ex)
      {
        response.Message = "Error: " + ex.Message;
        response.IsSuccess = false;

        return BadRequest(response);
      }
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<CaseEditResponseDto>>> Get([FromQuery] string id)
    {
      var response = new ResponseDto<CaseEditResponseDto>();
      try
      {
        string createdBy = User.GetClaim(AuthClaims.ACCOUNT_ID);
        var result = await _caseRepo.GetById(id);

        response.Result = new CaseEditResponseDto()
        {
          SerialNumber = result.SerialNumber,
          ServicePartner = result.ServicePartner,
        };

        response.Message = "Case (" + id + ") found";

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
    public async Task<ActionResult<ResponseDto>> Put([FromBody] CaseEditRequestDto requestDto)
    {
      var response = new ResponseDto();
      try
      {
        string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _caseRepo.EditAsync(requestDto, accountId);

        response.Message = "Case (" + requestDto.Id + ") Updated";

        return Ok(response);
      }
      catch (InvalidOperationException ex)
      {
        response.Message = ex.Message;
        response.IsSuccess = false;

        return BadRequest(response);
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
