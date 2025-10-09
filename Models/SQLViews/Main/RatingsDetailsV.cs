namespace CCIMS.Web.Models.SQLViews.Main
{
    public class RatingsDetailsV
    {
        public long Id { get; set; }
        public string CustomerId { get; set; }
        public long? CaseId { get; set; }
        public int RatingVal { get; set; }
        public string? Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CaseNumber { get; set; }
        public string? SerialNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? SpName { get; set; }
    }
}
