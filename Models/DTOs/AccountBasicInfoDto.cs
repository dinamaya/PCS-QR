using Microsoft.Build.ObjectModelRemoting;

namespace CCIMS.Web.Models.DTOs
{
  public class AccountBasicInfoDto
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string AccountType { get; set; }
  }
}
