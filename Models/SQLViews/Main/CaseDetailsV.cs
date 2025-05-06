using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class CaseDetailsV
{
	public long Id { get; set; }

	public string CaseNumber { get; set; } = null!;

	public string Description { get; set; } = null!;

	public string SpName { get; set; } = null!;

	public string SerialNumber { get; set; } = null!;

	public DateTime DateCreated { get; set; }

	public bool IsActive { get; set; }

	public string CustomerId { get; set; } = null!;
}
