using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class ActiveTierInfo
    {
        public int ActiveTier { get; set; }
        public string ActiveTierCategoryCode { get; set; }
        public DateTime? ActiveTierValidFrom { get; set; }
        public DateTime? ActiveTierValidTo { get; set; }
        public double? Points { get; set; }
    }
}
