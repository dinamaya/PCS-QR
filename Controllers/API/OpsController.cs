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
  [Authorize]
  public class OpsController : ControllerBase
  {
    private readonly IOperationsRepository _opsRepo;

    public OpsController(IOperationsRepository opsRepo)
    {
      _opsRepo = opsRepo;
    }

    [HttpPost("status"), Authorize]
    public async Task<ActionResult<ResponseDto>> Status([FromBody] StatusDto creationRequest)
    {
      var response = new ResponseDto();
      try
      {
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

		[HttpGet("status"), Authorize]
		public async Task<ActionResult<ResponseDto<StatusDto>>> Status([FromQuery] string id)
		{
			var response = new ResponseDto<StatusDto>();
			try
			{
				var status = await _opsRepo.GetStatusById(id);

				response.Result = status;
				response.Message = "Status (" + id + ") found";

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.Message = "Error: " + ex.Message;
				response.IsSuccess = false;

				return BadRequest(response);
			}
		}

		[HttpPut("status"), Authorize]
		public async Task<ActionResult<ResponseDto>> Status([FromBody] StatusEditRequestDto editRequestDto)
		{
			var response = new ResponseDto();
			try
			{
        string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
				await _opsRepo.EditAsync(editRequestDto, accountId);

				response.Message = "Status (" + editRequestDto.Id + ") Updated";

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.Message = "Error: " + ex.Message;
				response.IsSuccess = false;

				return BadRequest(response);
			}
		}

    [HttpDelete("status"), Authorize]
    public async Task<ActionResult<ResponseDto>> StatusDelete([FromQuery] string id)
    {
      var response = new ResponseDto();
      try
      {
				await _opsRepo.DeactivateAsync(id);
				response.Message = "Status (" + id + ") Deleted";

        return Ok(response);
      }
      catch (Exception ex)
      {
        response.Message = "Error: " + ex.Message;
        response.IsSuccess = false;

        return BadRequest(response);
      }
    }


    [HttpGet("status/commentable")]
		public async Task<ActionResult<ResponseDto>> IsCommentable([FromQuery] string id)
		{
			var response = new ResponseDto();
			try
			{
				response.Result = await _opsRepo.IsStatusCommentable(id);
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
	}
}
