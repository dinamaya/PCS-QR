namespace CCIMS.Web.Models.ViewModels
{
    public class RatingRowViewModel
    {
        public string Id { get; set; }                     
        public string CaseNumber { get; set; }             
        public string SerialNumber { get; set; }           
        public string CustomerId { get; set; }             
        public string CaseId { get; set; }                 
        public int RatingVal { get; set; }                 
        public string Comment { get; set; }                
        public DateTime DateCreated { get; set; }          
        public string CustomerName { get; set; }      
        public string ServicePartnerName { get; set; }     
    }

}
