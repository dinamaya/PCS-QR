namespace CCIMS.Web.Models.Complex
{
  public class EmailServiceConfig
  {
    public Dictionary<string, EmailCredential> Credentials { get; set; }
    public List<string> TestEmails { get; set; }
    public List<string> Bcc { get; set; }
  }
}
