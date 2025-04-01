using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public required string Role { get; set; }
        public string? ImgPath { get; set; }

        public ICollection<Task> TaskList { get; set; } = new List<Task>();
    }
}
