namespace CCIMS.Web.Models.Interfaces
{
  public interface ISysAdminSecurityDetails
  {
    public byte[] PrivateKey { get; }
    public byte[] PrivateIV { get; }
  }
}
