using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIMS.Web.App_Code._Globals.Constants;
using CCIMS.Web.Context;
using CCIMS.Web.Repositories.Interfaces;

namespace CCIMS.Web.Controllers.API
{
	[Route("api/qr")]
	[ApiController]
	public class QRController : ControllerBase
	{
		private readonly ITokenProvider _tokenProvider;
		private readonly ISecurityRepository _securityRepo;

		private readonly MainDbContext _mainDb;

    public QRController(ITokenProvider tokenProvider, MainDbContext mainDb, ISecurityRepository securityRepo)
    {
      _tokenProvider = tokenProvider;
      _mainDb = mainDb;
      _securityRepo = securityRepo;
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

      var token = _tokenProvider.GenerateToken(15, 2, data);
			return Ok(new { token });
		}
	}
}
