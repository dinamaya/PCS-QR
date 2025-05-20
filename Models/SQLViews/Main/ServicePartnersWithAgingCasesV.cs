using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class ServicePartnersWithAgingCasesV
{
  public string ServicePartnerName { get; set; } = null!;

  public long Id { get; set; }

  public string CaseNumber { get; set; } = null!;

  public string Description { get; set; } = null!;

  public string Status { get; set; } = null!;

  public string? Comments { get; set; }

  public string CustomerName { get; set; } = null!;

  public string SerialNumber { get; set; } = null!;

  public DateTime DateStatusUpdated { get; set; }

  public bool IsActive { get; set; }
}
