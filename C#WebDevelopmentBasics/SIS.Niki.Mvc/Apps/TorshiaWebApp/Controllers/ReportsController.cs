﻿namespace TorshiaWebApp.Controllers
{
    using System.Linq;
    using System.Globalization;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using ViewModels.Report;

    public class ReportsController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var reports = this.DbContext.Reports.Select(r => new ReportViewModel
            {
                Id = r.Id,
                Title = r.Task.Title,
                Level = r.Task.Sectors.Count(),
                Status = r.Status.ToString(),
            }).ToArray();

            return this.View(reports);
        }

        [Authorize("Admin")]
        public IHttpResponse Details(int id)
        {
            var report = this.DbContext.Reports.FirstOrDefault(r => r.Id == id);
            if (report == null)
            {
                return this.BadRequestErrorWithView($"Report with id {id} not found.");
            }

            var viewModel = new ReportDetailsViewModel()
            {
                Id = report.Id,
                TaskTitle = report.Task.Title,
                TaskLevel = report.Task.Sectors.Count(),
                Status = report.Status,
                ReportedOn = report.ReportedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Participants = report.Task.Participants,
                AffectedSectors = string.Join(", ", report.Task.Sectors.Select(s => s.Sector)),
                Reporter = report.Reporter.Username,
                TaskDescription = report.Task.Description,
            };

            if (report.Task.DueDate != null)
            {
                viewModel.DueDate = report.Task.DueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return this.View(viewName: "ReportDetails", model: viewModel);
        }
    }
}
