using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class LoyaltyCustomerEventsFeed
    {
        public List<CustomerProgramEventsFeed> ProgramFeeds { get; set; }
        public List<CustomerPointsEventsFeed> PointsFeeds { get; set; }
        public List<CustomerRewardEventsFeed> RewardFeeds { get; set; }
        public List<CustomerWishListEventsFeed> WishListFeeds { get; set; }
    }
}
