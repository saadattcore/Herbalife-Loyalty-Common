using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class RewardsTierGroup
    {
        public int Tier { get; set; }
        public List<LoyaltyRewardModel> Rewards { get; set; }
        public bool Redeemed { get; set; }
    }
}
