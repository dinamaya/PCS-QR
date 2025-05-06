using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
  public class AccountBasicInfoDto
  {
    [Required(ErrorMessage = "First name is required.")] public string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required.")] public string LastName { get; set; }
    [Required(ErrorMessage = "Username is required.")] public string Username { get; set; }
    [Required(ErrorMessage = "Email is required.")] public string Email { get; set; }
    [Required(ErrorMessage = "Account Type is required.")] public string AccountType { get; set; }
  }
}
