using System;

namespace HL.Loyalty.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        public Guid GoHlCustomerId { get; set; }
        public string LoyalityProgramId { get; set; }
        public string ContactId { get; set; }
        public string DistributorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Tier { get; set; }
        public decimal? Points { get; set; }
        public int? ShoppingTier { get; set; }
        public int? ActivityTier { get; set; }
        public double? ActivityNeededForNextLevel { get; set; }
        public double? ProductNeededForNextLevel { get; set; }
        public double? ProductPointsNeededThisMonth { get; set; }
        public int ConsecutiveMonthsAchieved { get; set; }
        public int? ProductMaxTier { get; set; }

    }
}
