using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.ViewModels;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/fetch")]
  [ApiController]
  public class DataFetchController : ControllerBase
  {
    private readonly IServicePartnerRepository _spRepo;

    public DataFetchController(IServicePartnerRepository spRepo)
    {
      _spRepo = spRepo;
    }

    [HttpGet("sp")]
    public async Task<ActionResult<ResponseDto<IEnumerable<DropdownOptionDto>>>> ServicePartners([FromQuery] string name)
    {
      var response = new ResponseDto<IEnumerable<DropdownOptionDto>>();
      try
      {
        response.Result = await _spRepo.GetDropdownOptionsByName(name);
        response.Message = response.Result.Count().ToString();

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
