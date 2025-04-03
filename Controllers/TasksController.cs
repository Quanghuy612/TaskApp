using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.DTO;

namespace TaskApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskAppContext db;

        public TasksController(TaskAppContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await (from task in db.Tasks
                               join user in db.Users on task.UserId equals user.Id into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               join project in db.Projects on task.ProjectId equals project.Id
                               select new TaskDTO
                               {
                                   Id = task.Id,
                                   Title = task.Title,
                                   Description = task.Description,
                                   DueDate = task.DueDate,
                                   Status = task.Status.ToString(),
                                   UserId = task.UserId,
                                   UserFullName = user != null ? user.FullName : "Unassigned",
                                   ProjectId = task.ProjectId,
                                   ProjectName = project.Name
                               })
                             .ToListAsync();

            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUser(int taskId, int userId)
        {
            try
            {
                var task = await db.Tasks.FindAsync(taskId);
                if (task == null)
                {
                    return Json(new { success = false, message = "Task not found" });
                }

                var userExists = await db.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                task.UserId = userId;
                task.Status = "InProgress";

                await db.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskDTO task)
        {
            var existingTask = await db.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);

            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Status = task.Status;

                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Update successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Task not found" });
            }
        }

        [HttpPost]
        public IActionResult UpdateStatus(int taskId, string status)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (task != null)
            {
                task.Status = status;
                db.SaveChanges();
                return Json(new { success = true, message = "Update successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Task not found" });
            }
        }

    }
}
