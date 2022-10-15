using System;

namespace HL.Loyalty.Models
{
    public class TierConfiguration
    {
        public Tier Tier { get; set; }

        public PointsCategoryType PointsType { get; set; }

        public string CountryCode { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public float GoalPoints { get; set; }

        public int AchievementsGoal { get; set; }
    }
}
