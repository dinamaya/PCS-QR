using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Models.DTOs
{
	public class QRTokenDto
	{
		public DateTime Expiry { get; set; }
		public int RemainingUses { get; set; }
		public string QRID { get; set; }
	}
}
