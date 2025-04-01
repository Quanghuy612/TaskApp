using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Models;
using static TaskApp.Controllers.Projects;

namespace TaskApp.Controllers
{
    public class Projects : Controller
    {
        private readonly TaskAppContext db;
        public Projects(TaskAppContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var project = db.Projects.Include(p => p.TaskList).ToList();

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public class TaskDTO
        {
            public required string Title { get; set; }
            public string? Description { get; set; }
            public string? DueDate { get; set; }
        }

        public class ProjectDTO : Project
        {
            public List<TaskDTO>? TaskList { get; set; }
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectDTO project)
        {
            if (project == null || string.IsNullOrEmpty(project.Name))
            {
                return Json(new { success = false, message = "Project name is required." });
            }

            db.Projects.Add(project);
            db.SaveChanges();

            if (project.TaskList != null && project.TaskList.Any())
            {
                foreach (var taskDto in project.TaskList)
                {
                    DateOnly? dueDate = null;
                    if (!string.IsNullOrEmpty(taskDto.DueDate))
                    {
                        if (DateOnly.TryParse(taskDto.DueDate, out DateOnly parsedDate))
                        {
                            dueDate = parsedDate;
                        }
                        else
                        {
                            return Json(new { success = false, message = "Invalid due date format !" });
                        }
                    }

                    var task = new Models.Task
                    {
                        Title = taskDto.Title,
                        Description = taskDto.Description,
                        DueDate = dueDate,
                        ProjectId = project.Id
                    };
                    db.Tasks.Add(task);
                }
            }

            db.SaveChanges();

            return Json(new { success = true, message = "Project created successfully!" });
        }

    }
}
