namespace TorshiaWebApp.ViewModels.Task
{
    public class TaskCreateViewModel
    {
        public string Title { get; set; }

        public string DueDate { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        // TODO: This is not ok
        public string Customers { get; set; }
        public string Marketing { get; set; }
        public string Finances { get; set; }
        public string Internal { get; set; }
        public string Management { get; set; }
    }
}
