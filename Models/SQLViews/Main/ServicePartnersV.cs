namespace CCIMS.Web.Models.SQLViews.Main;

public partial class ServicePartnersV
{
  public string? QrId { get; set; }

  public string SpId { get; set; } = null!;

  public DateTime? QrDateCreated { get; set; }

  public string SpName { get; set; } = null!;

  public string CompanyName { get; set; } = null!;

  public string ContactNumber { get; set; } = null!;

  public string Email { get; set; } = null!;

  public string ContactPerson { get; set; } = null!;

  public DateTime SpDateCreated { get; set; }

  public string? Creator { get; set; }
}
