namespace PandaWebApp.ViewModels.Receipts
{
    public class ReceiptDetailsViewModel
    {
        public int ReceiptNumber { get; set; }

        public string DeliveryAddress { get; set; }

        public string IssueDate { get; set; }

        public string Recipient { get; set; }

        public double PackageWeight { get; set; }

        public string PackageDescription { get; set; }

        public decimal Total { get; set; }
    }
}
