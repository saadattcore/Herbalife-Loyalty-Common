using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerWishListEventsFeed
    {
        public string EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string Sku { get; set; }
        public int Tier { get; set; }
        public string Category { get; set; }
        public string RewardName { get; set; }
        public string CountryCode { get; set; }
    }
}
