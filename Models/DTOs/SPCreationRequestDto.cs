using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
  public class SPCreationRequestDto
  {
    [Required(ErrorMessage = "Service Partner name is required.")] public string Name { get; set; }
    [Required(ErrorMessage = "Company name is required.")] public string CompanyName { get; set; }
    [Required(ErrorMessage = "Contact Number is required.")] public string ContactNumber { get; set; }
    [Required(ErrorMessage = "Email Address name is required.")] public string Email { get; set; }
    [Required(ErrorMessage = "Contact Person name is required.")] public string ContactPerson { get; set; }
  }
}
