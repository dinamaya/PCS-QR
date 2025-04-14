using Azure;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/sp")]
  [ApiController]
  public class ServicePartnerController : ControllerBase
  {
    private readonly IServicePartnerRepository _spRepo;
    public ServicePartnerController(IServicePartnerRepository spRepo)
    {
      _spRepo = spRepo;
		}

		[HttpPost, Authorize]
    public async Task<ActionResult<ResponseDto>> Post([FromBody] SPCreationRequestDto creationDto)
    {
			var response = new ResponseDto();
			try
			{
        string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _spRepo.CreateAsync(creationDto, accountId);

        response.Message = "Service Partner created successfully";

				return Ok(response);
      }
      catch (Exception ex)
      {
        response.Message = "Error: " + ex.Message;
				response.IsSuccess = false;
				
        return BadRequest(response);
			}
		} 

    [HttpGet, Authorize]
    public async Task<ActionResult<ResponseDto<SPEditResponseDto>>> Get([FromQuery] string id)
    {
			var response = new ResponseDto<SPEditResponseDto>();
			try
			{

				response.Result = await _spRepo.GetById(id) ?? throw new InvalidOperationException(Exceptions.Message.INVALID_SPREFERENCE);
        
        response.Message = "Service Partner (" + id+ ") found";
				
        return response;
			}
      catch (Exception ex)
      {

				response.Message = "Error: " + ex.Message;
				response.IsSuccess = false;

				return BadRequest(response);
			}
    }

    [HttpPut, Authorize]
    public async Task<ActionResult<ResponseDto>> Put([FromBody] SPEditRequestDto requestDto)
    {
			var response = new ResponseDto<SPEditResponseDto>();
      response.Result = null;
			
      try
			{
        string modifiedBy = User.GetClaim(AuthClaims.ACCOUNT_ID);

				await _spRepo.EditAsync(requestDto, modifiedBy);
        response.Message = "Service Partner Editted";
				
        return response;
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
