using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/customer")]
  [ApiController, Authorize(Roles = "SPA")]
  public class CustomerController : ControllerBase
  {
    private readonly ICustomerRepository _customRepo;

    public CustomerController(ICustomerRepository customRepo)
    {
      _customRepo = customRepo;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<CustomerEditResponseDto>>> Get([FromQuery] string id)
    {
      var response = new ResponseDto<CustomerEditResponseDto>();
      try
      {
        var result = await _customRepo.GetById(id);
        response.Result = new CustomerEditResponseDto()
        {
          FirstName = result.Firstname,
          LastName = result.Lastname,
          Email = result.Email,
          ContactNo = result.ContactNo,
          Address = result.Address,
        };

        response.Message = "Customer (" + id + ") found";

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
    public async Task<ActionResult<ResponseDto>> Put([FromBody] CustomerEditRequestDto requestDto)
    {
      var response = new ResponseDto();
      try
      {
        string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _customRepo.EditAsync(requestDto, accountId);

        response.Message = "Customer (" + requestDto.Id + ") Updated";

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
