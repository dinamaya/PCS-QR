using CCIMS.Web.Models.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIMS.Web.Models.Entities.Main
{
    public class Rating : BigEntity
    {
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        [ForeignKey("Case")]
        public long CaseId { get; set; }
        public string CaseNumber { get; set; }
        public int RatingVal { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Case Case { get; set; }
    }
}