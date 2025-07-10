using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class WeeklyClosedCasesChartV
{
    public DateOnly? CaseDate { get; set; }

    public string? DayOfWeek { get; set; }

    public int TotalClosedCases { get; set; }
}
