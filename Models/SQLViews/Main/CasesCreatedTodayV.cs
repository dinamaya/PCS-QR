using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class CasesCreatedTodayV
{
    public long Id { get; set; }

    public string CaseNumber { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

	public string FirstName { get; set; } = null!; 

	public string LastName { get; set; } = null!;  

	public string Description { get; set; } = null!;

    public string QrcodeId { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public string? ModifiedBy { get; set; }

    public DateTime DateModified { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsActive { get; set; }
}
