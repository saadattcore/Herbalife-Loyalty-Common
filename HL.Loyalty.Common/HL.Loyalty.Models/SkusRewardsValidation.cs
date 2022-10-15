using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class SkusRewardsValidation
    {
        public Guid Id { get; set; }
        public string DistributorId { get; set; }
        public CustomerTypes CustomerIdType { get; set; }
        public List<string> Skus { get; set; }
        public string Locale { get; set; }
        public decimal TotalCartVolumenPoints { get; set; }
    }

}
