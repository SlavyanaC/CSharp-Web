namespace PandaWebApp.Controllers
{
    using System.Globalization;
    using System.Linq;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Receipts;

    public class ReceiptsController : BaseController
    {
        [Authorize]
        public IHttpResponse Index()
        {
            var receipts = this.DbContext.Receipts.Where(r => r.Recipient.Username == this.User.Username)
                .Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    Fee = r.Fee,
                    IssueDate = r.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Recipient = r.Recipient.Username,
                }).ToArray();

            return this.View(receipts);
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var receipt = this.DbContext.Receipts.FirstOrDefault(r => r.Id == id);
            if (receipt == null)
            {
                return this.BadRequestErrorWithView($"Receipt with id {id} not found");
            }

            if (receipt.Recipient.Username != this.User.Username)
            {
                return this.BadRequestError("You are not the owner of that receipt.");
            }

            var viewModel = new ReceiptDetailsViewModel
            {
                ReceiptNumber = receipt.Id,
                IssueDate = receipt.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                DeliveryAddress = receipt.Package.ShippingAddress,
                PackageWeight = receipt.Package.Weight,
                PackageDescription = receipt.Package.Description,
                Recipient = receipt.Recipient.Username,
                Total = receipt.Fee,
            };

            return this.View(viewModel);
        }
    }
}
