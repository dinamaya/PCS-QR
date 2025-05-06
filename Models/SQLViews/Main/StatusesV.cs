namespace CCIMS.Web.Models.SQLViews.Main
{
  public partial class StatusesV
  {
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsCommentable { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public bool IsActive { get; set; }

    public string Creator { get; set; } = null!;
  }

}
