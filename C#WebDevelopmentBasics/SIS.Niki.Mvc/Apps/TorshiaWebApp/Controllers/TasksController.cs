namespace TorshiaWebApp.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using Models;
    using Models.Enums;
    using ViewModels.Task;

    public class TasksController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var task = this.DbContext.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return this.BadRequestErrorWithView($"Task with id {id} does not exist.");

            }

            var taskDetailsViewModel = new TaskDetailsViewModel
            {
                Title = task.Title,
                Level = task.Sectors.Count(),
                Participants = task.Participants,
                Description = task.Description,
                AffectedSectors = string.Join(", ", task.Sectors.Select(s => s.Sector)),
            };

            if (task.DueDate != null)
            {
                taskDetailsViewModel.DueDate = task.DueDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return this.View(viewName: "TaskDetails", model: taskDetailsViewModel);
        }

        [Authorize]
        public IHttpResponse Create()
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username);
            if (user.Role != Role.Admin)
            {
                return this.BadRequestError("You are not authorized to access this page.");
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Create(TaskCreateViewModel model)
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username);
            if (user.Role != Role.Admin)
            {
                return this.BadRequestError("You are not authorized to access this page.");
            }

            var task = new Task
            {
                Title = model.Title,
                IsReported = false,
                Participants = model.Participants,
                Description = model.Description,
            };

            if (model.DueDate != "")
            {
                task.DueDate = DateTime.Parse(model.DueDate);
            }

            var sectors = new[] { model.Marketing, model.Management, model.Internal, model.Finances, model.Customers, };
            foreach (var sector in sectors)
            {
                if (Enum.TryParse(sector, out Sector currentSector))
                {
                    var taskSector = new TaskSector()
                    {
                        Task = task,
                        Sector = currentSector,
                    };
                    task.Sectors.Add(taskSector);
                    this.DbContext.TasksSectors.Add(taskSector);
                }
            }

            this.DbContext.Tasks.Add(task);
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

        [Authorize]
        public IHttpResponse Report(int id)
        {
            var task = this.DbContext.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return this.BadRequestErrorWithView($"Task with id {id} does not exist.");
            }

            task.IsReported = true;

            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username);
            var report = new Report()
            {
                TaskId = id,
                Reporter = user,
                ReportedOn = DateTime.UtcNow.Date,
            };

            var random = new Random();
            var chance = random.Next(1, 101);
            if (chance <= 25)
            {
                report.Status = Status.Completed;
            }
            else
            {
                report.Status = Status.Archived;
            }

            this.DbContext.Reports.Add(report);
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
    }
}
