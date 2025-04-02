using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCIMS.Web.Models.DTOs
{
  public class QRCreationRequestDto
  {
    public string ServicePartnerId { get; set; }
    public string Description { get; set; }
  }
}
