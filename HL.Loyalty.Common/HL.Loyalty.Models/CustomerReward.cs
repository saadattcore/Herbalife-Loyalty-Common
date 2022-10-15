using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerReward
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? RewardDate { get; set; }
        public Guid RewardId { get; set; }
        public string RewardName { get; set; }
        public string RewardType { get; set; }
        public string RewardTypeDescription { get; set; }
        public string Sku { get; set; }
        public Guid GOHLCustomerId { get; set; }
        public int Tier { get; set; }

    }
}
