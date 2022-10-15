using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class ProgramModel
    {
        public Guid ProgramId { get; set; }

        public string ProgramName { get; set; }

        public string DistributorId { get; set; }

        public string CountryCodeISO { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool ActiveCustomerEnrolled { get; set; }
        

        public List<ActivityModel> Activities { get; set; }

        public List<LoyaltyRewardModel> ActivityRewardsGifts { get; set; }

        public virtual List<LoyaltyRewardModel> PurchaseRewardsGifts { get; set; }

        public virtual Guid Id { get; set; }

        public bool EnableShoppingRewards { get; set; }

        public bool EnableActivityRewards { get; set; }


    }
}
