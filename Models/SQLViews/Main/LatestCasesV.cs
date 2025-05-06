using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class LatestCasesV
{
  public long TransId { get; set; }

  public long CaseId { get; set; }

  public string CaseNumber { get; set; } = null!;

  public string CustomerId { get; set; } = null!;

  public string Description { get; set; } = null!;

  public string SerialNumber { get; set; } = null!;

  public string? Comments { get; set; }

  public string StatusId { get; set; } = null!;

  public string Status { get; set; } = null!;

  public bool IsCommentable { get; set; }

  public DateTime DateStatusUpdated { get; set; }

  public string ServicePartnerName { get; set; } = null!;

  public string ServicePartnerId { get; set; } = null!;

  public string CustomerFirstName { get; set; } = null!;

  public string CustomerLastName { get; set; } = null!;
}