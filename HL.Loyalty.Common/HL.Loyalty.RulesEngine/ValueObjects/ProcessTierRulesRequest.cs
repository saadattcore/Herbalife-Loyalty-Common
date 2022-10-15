using HL.Loyalty.Models;
using System.Collections.Generic;

namespace HL.Loyalty.RulesEngine.ValueObjects
{
    public class ProcessTierRulesRequest
    {
        public IList<Order> Orders { get; set; }

        public string CountryCode { get; set; }
    }
}
