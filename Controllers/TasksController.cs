using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Data;

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
            var tasks = await db.Tasks.ToListAsync();
            return View(tasks);
        }
    }
}
