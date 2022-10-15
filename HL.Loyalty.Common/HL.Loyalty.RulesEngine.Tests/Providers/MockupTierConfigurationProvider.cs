using HL.Loyalty.Models;
using HL.Loyalty.RulesEngine.Interfaces;
using System.Collections.Generic;

namespace HL.Loyalty.RulesEngine.Tests.Providers
{
    public class MockupTierConfigurationProvider : ITierConfigurationProvider
    {
        private List<TierConfiguration> _tiers { get; set; }
        public MockupTierConfigurationProvider(List<TierConfiguration> tiers)
        {
            _tiers = tiers;
        }
        public IList<TierConfiguration> Load(PointsCategoryType category,string locale)
        {
            return _tiers;
        }

        
    }
}
