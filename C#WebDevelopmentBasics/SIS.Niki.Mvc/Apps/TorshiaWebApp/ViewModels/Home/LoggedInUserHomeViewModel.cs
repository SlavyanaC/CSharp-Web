namespace TorshiaWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using Task;

    public class LoggedInUserHomeViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}
