using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskApp.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateOnly? DueDate { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
    }
}
