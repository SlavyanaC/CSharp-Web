namespace PandaWebApp.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using Models;
    using Models.Enums;
    using ViewModels.Packages;

    public class PackagesController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var package = this.DbContext.Packages.Find(id);
            if (package == null)
            {
                return this.BadRequestErrorWithView($"Package with id {id} not found");
            }

            if (this.User.Role != "Admin" && package.Recipient.Username != this.User.Username)
            {
                return this.BadRequestError("You are not the owner of that package.");
            }

            var deliveryDate = string.Empty;
            if (package.Status == Status.Pending || package.EstimatedDeliveryDate.Value == null)
            {
                deliveryDate = "N/A";
            }
            else if (package.Status == Status.Shipped)
            {
                deliveryDate = package.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                deliveryDate = "Delivered";
            }

            var model = new PackageDetailsViewModel
            {
                ShippingAddress = package.ShippingAddress,
                Status = package.Status.ToString(),
                Weight = package.Weight,
                EstimatedDeliveryDate = deliveryDate,
                Description = package.Description,
                Recipient = package.Recipient.Username,
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var users = this.DbContext.Users.Select(u => u.Username).ToArray();
            return this.View(users);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(PackageCreateViewModel model)
        {
            var recipient = this.DbContext.Users.FirstOrDefault(u => u.Username == model.Recipient);
            if (recipient == null)
            {
                return this.BadRequestErrorWithView("Please select recipient");
            }

            var package = new Package
            {
                Description = model.Description,
                Weight = model.Weight,
                ShippingAddress = model.Address,
                RecipientId = recipient.Id,
                Status = Status.Pending,
                EstimatedDeliveryDate = null,
            };

            this.DbContext.Packages.Add(package);
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            var pending = this.DbContext.Packages.Where(p => p.Status == Status.Pending).ToArray();
            return this.View(pending);
        }

        [Authorize("Admin")]
        public IHttpResponse Ship(int id)
        {
            var package = this.DbContext.Packages.FirstOrDefault(p => p.Id == id);
            if (package == null)
            {
                return this.BadRequestErrorWithView($"Package with id {id} not found.");
            }
            package.Status = Status.Shipped;

            var random = new Random();
            var shippingDaysRandom = random.Next(20, 41);
            var shippingDate = DateTime.UtcNow.AddDays(shippingDaysRandom);

            package.EstimatedDeliveryDate = shippingDate;
            this.DbContext.Packages.Update(package);
            this.DbContext.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            var shipped = this.DbContext.Packages.Where(p => p.Status == Status.Shipped).Select(p => new PackageDetailsViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Weight = p.Weight,
                EstimatedDeliveryDate = p.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Recipient = p.Recipient.Username,
            }).ToArray();

            return this.View(shipped);
        }

        [Authorize("Admin")]
        public IHttpResponse Deliver(int id)
        {
            var package = this.DbContext.Packages.FirstOrDefault(p => p.Id == id);
            if (package == null)
            {
                return this.BadRequestErrorWithView($"Package with id {id} not found.");
            }
            package.Status = Status.Delivered;

            this.DbContext.Packages.Update(package);
            this.DbContext.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Acquire(int id)
        {
            var package = this.DbContext.Packages.FirstOrDefault(p => p.Id == id);
            if (package == null)
            {
                return this.BadRequestErrorWithView($"Package with id {id} not found.");
            }

            package.Status = Status.Acquired;
            DbContext.Packages.Update(package);

            var recipient = this.DbContext.Users.FirstOrDefault(u => u.Username == package.Recipient.Username);
            var receipt = new Receipt
            {
                Package = package,
                Recipient = recipient,
                IssuedOn = DateTime.UtcNow,
                Fee = (decimal)package.Weight * 2.67m,
            };

            this.DbContext.Receipts.Add(receipt);
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            var delivered = this.DbContext.Packages.Where(p => p.Status == Status.Delivered || p.Status == Status.Acquired).ToArray();
            return this.View(delivered);
        }
    }
}
