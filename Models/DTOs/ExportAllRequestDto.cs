namespace CCIMS.Web.Models.DTOs
{
  public class ExportAllRequestDto
  {
    public string? Category { get; set; }
    public string? CategoryValue { get; set; }
    public string? ServicePartner { get; set; }
    public string? SearchTerm { get; set; }
    public string? DateRange { get; set; }
  }
}
