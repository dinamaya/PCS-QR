namespace CCIMS.Web.Models.ViewModels
{
  public class CaseRowViewModel
  {
    public string Id { get; set; }
    public string CaseNumber { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Comments { get; set; }
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public string ServicePartnerName { get; set; }
    public string SerialNumber { get; set; }
    public string DateCreated { get; set; }
    public string DateUpdated { get; set; }
    public string DaysAged { get; set; }
    public string IsActive { get; set; }
    public string UpdatedByFullName { get; set; }
  }
}
