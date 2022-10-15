using System;

namespace HL.Loyalty.Models
{
    public class LoyaltyRewardModel
    {
        public Guid RewardId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryDescription { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public string CountryCodeIso { get; set; }
        public Decimal? Points { get; set; }
        public Decimal? Price { get; set; }
        public virtual string Image { get; set; }
        public int? Tier { get; set; }
        public bool Selected { get; set; }
        public bool Redeemed { get; set; }
        public string RedemptionStatusCode { get; set; }

    }
}
