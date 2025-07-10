namespace CCIMS.Web.Models.Complex
{
  public class EmailCredential
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public string SenderEmailAddress { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ReplyAddress { get; set; }
  }
}
