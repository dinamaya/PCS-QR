using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class TotalOpenCasesV
{
    public string Status { get; set; } = null!;

    public int? TotalOpenCases { get; set; }
}
