using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class LatestCasesSubmissionV
{
    public long CaseId { get; set; }

    public string CaseNumber { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string ServicePartnerName { get; set; } = null!;
}
