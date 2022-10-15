using HL.Loyalty.Models;
using System.Collections.Generic;

namespace HL.Loyalty.RulesEngine.Interfaces
{
    public interface ITierConfigurationProvider
    {
         IList<TierConfiguration> Load(PointsCategoryType category, string countryCode);
    }
}
