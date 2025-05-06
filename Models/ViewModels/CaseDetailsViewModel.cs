namespace CCIMS.Web.Models.ViewModels
{
	public class CaseDetailsViewModel
	{
		public string Id { get; set; }
		public string CustomerId { get; set; }
		public DateTime DateCreated { get; set; }
		public string CaseNumber { get; set; }
		public string ServicePartner { get; set; }
		public string SerialNumber { get; set; }
		public string Description { get; set; }
	}
}
