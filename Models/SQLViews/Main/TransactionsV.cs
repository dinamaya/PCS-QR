using System;
using System.Collections.Generic;

namespace CCIMS.Web.Models.SQLViews.Main;

public partial class TransactionsV
{
  public long Id { get; set; }

  public long CaseId { get; set; }

  public string StatusId { get; set; } = null!;

  public string Status { get; set; } = null!;

  public string? Comments { get; set; }

  public DateTime DateCreated { get; set; }

  public bool IsCommentable { get; set; }

  public bool IsActive { get; set; }

  public string? CreatorFirstName { get; set; }

  public string? CreatorLastName { get; set; }
}
