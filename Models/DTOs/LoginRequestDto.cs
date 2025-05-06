using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs;

public class LoginRequestDto
{
	[Required(ErrorMessage = "Please enter a valid username.")] public string Username { get; set; }
	[Required(ErrorMessage = "Password is required.")] public string PlaintextPassword { get; set; }
	public bool IsRemembered { get; set; } = false;
}
