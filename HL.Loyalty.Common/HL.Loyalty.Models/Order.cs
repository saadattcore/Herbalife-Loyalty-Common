using System;

namespace HL.Loyalty.Models
{
    public class Order
    {
        public Transaction Transaction { get; set; }

        public Guid Id { get; set; }

        public string OrderId { get; set; }

        public string HMSReceiptId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public float Points { get; set; }

        public float Volume { get; set; }

        public string CountryCode { get; set; }

        public DateTime CreatedUTC { get; set; }
    }
}
