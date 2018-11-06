namespace PandaWebApp.ViewModels.Receipts
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public string IssueDate { get; set; }

        public string Recipient { get; set; }
    }
}
