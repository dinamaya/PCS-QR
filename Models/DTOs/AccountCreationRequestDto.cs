using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
	public class AccountCreationRequestDto : AccountBasicInfoDto
  {
    [Required(ErrorMessage = "Password is required.")] public string Password { get; set; }
    [Required(ErrorMessage = "Retype Password is required.")] public string RetypePass { get; set; }
	}
}
