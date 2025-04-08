using CCIMS.Web.Models.Abstracts;
using CCIMS.Web.Models.Interfaces;

namespace CCIMS.Web.Models.Entities.Main
{
	public class Case : BigEntity, IModifiableByUser, ICreatable, IActivatable
	{
		public string CaseNumber { get; set; }
		public string CustomerID { get; set; }
		public string Description { get; set; }
		public string QRCodeId { get; set; }
		public string SerialNumber { get; set; }

		public string? ModifiedBy { get; set; }
		public DateTime DateModified { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsActive { get; set; }


		public virtual QRCode QRCode { get; set; }
	}
}
