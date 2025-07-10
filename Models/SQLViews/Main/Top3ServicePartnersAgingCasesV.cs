using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class Top3ServicePartnersAgingCasesV
{
    public string ServicePartnerName { get; set; } = null!;

    public int? AgingCaseCount { get; set; }
}
