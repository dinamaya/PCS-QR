namespace CCIMS.Web.Models.DTOs
{
	public class AccountCreationRequestDto
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Type { get; set; }
		public string Password { get; set; }
		public string RetypePass { get; set; }
	}
}
