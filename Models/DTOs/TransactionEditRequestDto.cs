using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
  public class TransactionEditRequestDto
  {
    [Required(ErrorMessage = "Transaction ID is required")] public string Id { get; set; }
    [Required(ErrorMessage = "Remarks is required")] public string Remarks { get; set; }
  }
}
