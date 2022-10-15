using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerPointsEventsFeed
    {
        public string EventType { get; set; }
        public string Category { get; set; }
        public string TransactionType { get; set; }
        public DateTime EventDate { get; set; }
        public decimal? Points { get; set; }
        public decimal TotalPoints { get; set; }
        public decimal? ActityPoints { get; set; }
        public int? Tier { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public string OrderNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal VolumePoints { get; set; }
        public Guid TrnHeaderID { get; set; }
    }
}
