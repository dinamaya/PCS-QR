using System.Text;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Complex
{
  public class SysAdminSecurityDetails : ISysAdminSecurityDetails
  {
    public string Key { get; set; }
    public string IV { get; set; }

    public byte[] PrivateKey => Encoding.UTF8.GetBytes(Key);
    public byte[] PrivateIV => Encoding.UTF8.GetBytes(IV);
  }
}
