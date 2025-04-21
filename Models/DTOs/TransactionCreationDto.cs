namespace CCIMS.Web.Models.DTOs
{
  public class TransactionCreationDto
  {
    public long CaseId { get; set; }
    public string Comments { get; set; }
    public string StatusId { get; set; }
  }
}
