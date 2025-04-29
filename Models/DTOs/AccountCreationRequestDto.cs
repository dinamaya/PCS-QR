namespace CCIMS.Web.Models.DTOs
{
	public class AccountCreationRequestDto : AccountBasicInfoDto
  {
		public string Password { get; set; }
		public string RetypePass { get; set; }
	}
}
