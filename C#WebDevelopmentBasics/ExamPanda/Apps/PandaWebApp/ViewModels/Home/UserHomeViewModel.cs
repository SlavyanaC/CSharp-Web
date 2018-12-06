namespace PandaWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Package;

    public class UserHomeViewModel
    {
        public IEnumerable<PackageHomeIndex> Pending { get; set; }

        public IEnumerable<PackageHomeIndex> Shipped { get; set; }

        public IEnumerable<PackageHomeIndex> Delivered { get; set; }
    }
}
