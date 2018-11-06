namespace PandaWebApp.Models
{
    using System;
    using Enums;

    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        public int ReceiptId { get; set; }

        public virtual Receipt Receipt { get; set; }
    }
}