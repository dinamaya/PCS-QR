namespace CCIMS.Web.Models.DTOs
{
	public class AccountEditRequestDto : AccountBasicInfoDto
	{
		public string Id { get; set; }
		public string PasswordResetType { get; set; }
		public string Password { get; set; }
		public string RetypePass { get; set; }
	}
}
