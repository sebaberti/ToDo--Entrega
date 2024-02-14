using System.ComponentModel.DataAnnotations;

namespace ToDoList_API.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Completed { get; set; }

        public DateTime? DeadLine { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
