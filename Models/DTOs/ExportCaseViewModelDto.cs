namespace CCIMS.Web.Models.DTOs
{
    public class ExportCaseViewModelDto
    {
        public string CaseNumber { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string CustomerName { get; set; }
        public string ServicePartner { get; set; }
        public string SerialNumber { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string DaysAged { get; set; }
    }
}
