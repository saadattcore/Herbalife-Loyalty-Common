using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerShoppingPoints
    {
        public int ProductCurrentTier { get; set; }
        public decimal? ProductPoints { get; set; }
        public int? ProductNextTier { get; set; }
        public double? ProductNextTierPoints { get; set; }
        public double? ProductPointsNeededNextLevel { get; set; }
        public int ConsecutiveMonthsAchieved { get; set; }
        public int? ProductMaxTier { get; set; }
        public bool FiftyPointFlag { get; set; }
        public List<RewardsTierGroup> RewardsGroups { get; set; }
        public bool CanMoveToNextLevel { get; set; }

    }
}
