using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
  public class CaseEditResponseDto
  {
    [Required(ErrorMessage = "Serial Number is Required")] public string SerialNumber { get; set; }
    [Required(ErrorMessage = "Service Partner is Required")] public string ServicePartner { get; set; }
  }
}
