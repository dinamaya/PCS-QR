using System.ComponentModel.DataAnnotations;

namespace CCIMS.Web.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public string Token { get; set; }
        public string CaseNumber { get; set; }
        public int Rating { get; set; }
        [MaxLength(length: 1500, ErrorMessage = "Maximum number (1500) of characters reached")] public string Comment { get; set; }
    }
}