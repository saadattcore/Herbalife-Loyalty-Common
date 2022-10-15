using HL.Loyalty.Common;

namespace HL.Loyalty.RulesEngine.ValueObjects
{
    public class ProcessTierRulesResponse
    {
        public LoyaltyProcessStatus Status { get; set; }

        public int Level { get; set; }

        public float LevelPoints { get; set; }
    }
}
