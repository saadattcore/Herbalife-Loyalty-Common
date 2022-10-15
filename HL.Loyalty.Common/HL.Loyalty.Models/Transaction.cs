using System;

namespace HL.Loyalty.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public Guid ProgramId { get; set; }

        public TransactionType Type { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime CreatedUTC { get; set; }

        public DateTime ModifiedUTC { get; set; }
    }
}
