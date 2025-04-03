namespace TaskApp.DTO
{
    public class TaskDTO
    {
        // Task properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly? DueDate { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public string Status { get; set; }

        public string UserFullName { get; set; }
        public string ProjectName { get; set; }
    }
}
