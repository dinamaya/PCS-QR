using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Main
{
  public class QRCode : HashedEntity, ICreatable, IModifiable, IActivatable
  {
		public QRCode() : base("QRC") {}

    public string ServicePartnerId { get; set; }
    public string Description { get; set; }

    public DateTime DateCreated { get;  set; }
    public DateTime DateModified { get; set; }
    public bool IsActive { get; set; }
  }
}
