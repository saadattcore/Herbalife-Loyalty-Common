using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class CustomerDetailModel
    {
        public Guid CustomerId { get; set; }
        public Guid GoHLCustomerId { get; set; }
        public Guid ProgramId { get; set; }
        public bool EnableShoppingRewards { get; set; }
        public bool EnableActivityRewards { get; set; }
        public DateTime? CustomerStartDate { get; set; }
        public DateTime? CustomerEndDate { get; set; }
        public bool ActiveProgram
        {
            get
            {
                return (Guid.Empty != ProgramId);
            }
        }
        public CustomerActivityPoints ActivityPoints { get; set; }
        public CustomerShoppingPoints ShoppingPoints { get; set; }
        public List<LoyaltyRewardModel> Rewards { get; set; }
        public List<ActiveTierInfo> TierInfo { get; set; }

    }
}
