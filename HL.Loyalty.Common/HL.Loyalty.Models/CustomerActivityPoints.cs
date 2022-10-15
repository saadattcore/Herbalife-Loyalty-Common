using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerActivityPoints
    {
        public int ActivityCurrentTier { get; set; }
        public decimal? ActivityPoints { get; set; }
        public int? ActivityNextTier { get; set; }
        public double? ActivityNextTierPoints { get; set; }
        public double? ActivityPointsNeededNextLevel { get; set; }
        public bool AbleToRedeem { get; set; }
        public List<RewardsTierGroup> RewardsGroups { get; set; }

    }
}
