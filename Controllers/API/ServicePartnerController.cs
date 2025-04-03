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
    private ResponseDto _response;
    public ServicePartnerController(IServicePartnerRepository spRepo)
    {
      _spRepo = spRepo;
    }

    [HttpPost("create"), Authorize]
    public async Task<ResponseDto> Create([FromBody] SPCreationRequestDto creationDto)
    {
      try
      {
        var date = DateTime.Now;
        string accountId = User.GetClaim(AuthClaims.ACCOUNT_ID);
        await _spRepo.CreateAsync(new ServicePartner()
        {
          Name = creationDto.Name,
          CompanyName = creationDto.CompanyName,
          ContactNumber = creationDto.ContactNumber,
          Email = creationDto.Email,
          ContactPerson = creationDto.ContactPerson,
          CreatedBy = accountId,
          DateCreated = date,
          DateModified = date,
          IsActive = true,
        });

        return new ResponseDto()
        {
          Result = null,
          Message = "Created Service Partner"
        };
      }
      catch (Exception ex)
      {
        return new ResponseDto()
        {
          Result = null,
          Message = "Error: " + ex.Message
        };
      }
    }
  }
}
