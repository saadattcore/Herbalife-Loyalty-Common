using System;

namespace HL.Loyalty.Models
{
    public class ActivityModel
    {
        public Guid ActivityId { get; set; }
        public string CountryCodeIso { get; set; }
        public string ActivityCode { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal? Points { get; set; }
        public bool IsUpdatedTest { get; set; }
    }
}
