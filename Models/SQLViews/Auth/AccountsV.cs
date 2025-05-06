using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Auth;

public partial class AccountsV
{
	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string? Email { get; set; }

	public string? UserName { get; set; }

	public DateTime DateModified { get; set; }

	public DateTime DateCreated { get; set; }

	public string PersonId { get; set; }

	public string AccountId { get; set; } = null!;

	public string? RoleName { get; set; }

	public string RoleId { get; set; } = null!;
}
