using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Models;

namespace TaskApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly TaskAppContext db;
        public UsersController(TaskAppContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await db.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

                if (existingUser != null)
                {
                    return Json(new { success = false, message = "User already exists." });
                }
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Json(new { success = true, message = "User added successfully!", user });
            }
            return Json(new { success = false, message = "Invalid data." });
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            if (id <= 0) return BadRequest("Invalid user ID.");

            var user = db.Users.Include(u => u.TaskList).FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user);
        }
        public class UserDto
        {
            public int Id { get; set; }
            public required string FullName { get; set; }
            public required string Email { get; set; }
            public required string Address { get; set; }
            public required string Role { get; set; }
        }

        [HttpPost]
        public IActionResult UpdateUser([FromBody] UserDto updatedUser)
        {
            var user = db.Users.Find(updatedUser.Id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.Address = updatedUser.Address;
            user.Role = updatedUser.Role;

            db.SaveChanges();

            return Json(new { success = true, message = "User updated successfully!" });
        }


        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Json(new { success = true, message = "User deleted successfully" });
        }

    }
}
