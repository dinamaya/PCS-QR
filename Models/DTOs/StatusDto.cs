using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.DTOs
{
  public class StatusDto
  {
    [Required(ErrorMessage = "Status name is required")]public string Name { get; set; }
    public bool IsCommentable { get; set; }
  }
}
