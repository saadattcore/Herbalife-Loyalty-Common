using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class ProgramActivity
    {
        public Guid ProgramActivityId { get; set; }
        public Guid ProgramId { get; set; }
        public Guid ActivityId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string ActivityCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Points { get; set; }
    }
}
