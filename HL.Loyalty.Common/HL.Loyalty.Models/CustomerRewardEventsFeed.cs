using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerRewardEventsFeed
    {
        public string RedemptionStatus { get; set; }
        public string RewardName { get; set; }
        public DateTime EventDate { get; set; }
        public int Tier { get; set; }
        public string Category { get; set; }
        public string OrderNumber { get; set; }
    }
}
