using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Repositories.Interfaces;
using CCIMS.Web.Models.DTOs;
using CCIMS.Web.Models.Entities.Main;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;
using Azure;

namespace CCIMS.Web.Controllers.API
{
	[Route("api/qr")]
	[ApiController]
	public class QRController : ControllerBase
	{
		private readonly ITokenProvider _tokenProvider;
		private readonly ISecurityRepository _securityRepo;
		private readonly IQRRepository _qrRepo;

		private readonly MainDbContext _mainDb;

		public QRController(ITokenProvider tokenProvider, MainDbContext mainDb, ISecurityRepository securityRepo, IQRRepository qrRepo)
		{
			_tokenProvider = tokenProvider;
			_mainDb = mainDb;
			_securityRepo = securityRepo;
			_qrRepo = qrRepo;
		}

		[HttpPost("token")]
		public async Task<IActionResult> GetToken([FromBody] string data)
		{
			if (string.IsNullOrWhiteSpace(data))
				return BadRequest("Invalid data.");
		
			string orgQrId = await _securityRepo.DecryptIDAsync(data);
      bool doesExist = await _mainDb.QRCodes.AnyAsync(q => q.Id == orgQrId);
      if (!doesExist)
        throw new Exception(Exceptions.Message.INVALID_QRREFERENCE);

      var token = _tokenProvider.Generate(data);
			return Ok(new { token });
		}

		[HttpGet]
		public async Task<ActionResult<ResponseDto<byte[]>>> Get([FromQuery] string? id=null)
		{
      var _response = new ResponseDto<byte[]>();
      try
      {
				if (string.IsNullOrEmpty(id))
				{
					var placeholderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/qr_placeholder.png");
					var placeholderBytes = await System.IO.File.ReadAllBytesAsync(placeholderPath);

					_response.Result = placeholderBytes;
					_response.Message = "No QR found, returning placeholder.";
					return Ok(_response);
				}

				var qrResult = await _qrRepo.GetById(id);
				_response.Result = qrResult;
				_response.Message = "QR generated successfully";

				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.Message = "Error: " + ex.Message;
				_response.IsSuccess = false;

				return BadRequest(_response);
			}
		}

	}
}
