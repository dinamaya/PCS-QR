using CCIMS.Web.App_Code._Globals;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.App_Code._Globals.Extensions;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Controllers.API
{
  [Route("api/ocr")]
  [ApiController]
  public class OCRController : ControllerBase
  {
    private readonly FileManager _fileManager;
    private readonly ICameraRepository _cameraRepo;

    public OCRController(FileManager fileManager, ICameraRepository cameraRepo)
    {
      _fileManager = fileManager;
      _cameraRepo = cameraRepo;
    }

    [HttpPost("recognize")]
    public async Task<ActionResult<ResponseDto<string>>> Recognize([FromForm] IFormFile file)
    {
      var response = new ResponseDto<string>();
      try
      {
        string filePath = await _fileManager.UploadAttachmentAsync(file);
        
        response.Result = await _cameraRepo.ExtractText(filePath);
        response.Message = "Image uploaded successfully";

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
