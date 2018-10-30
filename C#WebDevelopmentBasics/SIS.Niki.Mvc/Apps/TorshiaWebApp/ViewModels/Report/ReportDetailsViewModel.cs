namespace TorshiaWebApp.ViewModels.Report
{
    using Models.Enums;

    public class ReportDetailsViewModel
    {
        public int Id { get; set; }

        public string TaskTitle { get; set; }

        public int TaskLevel { get; set; }

        public Status Status { get; set; }

        public string DueDate { get; set; }

        public string ReportedOn { get; set; }

        public string Reporter { get; set; }

        public string Participants { get; set; }

        public string AffectedSectors { get; set; }

        public string TaskDescription { get; set; }
    }
}
