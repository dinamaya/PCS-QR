using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class TopAgingCasesV
{
    public long CaseId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string CaseNumber { get; set; } = null!;

    public int? NumberOfDays { get; set; }

    public string ServicePartnerName { get; set; } = null!;
}
