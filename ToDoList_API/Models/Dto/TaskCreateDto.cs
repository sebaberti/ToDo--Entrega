using System.ComponentModel.DataAnnotations;

namespace ToDoList_API.Models.Dto
{
    public class TaskCreateDto
    {
        
        [Required]
        public string Name { get; set; }
        public bool Completed { get; set; }

        public DateTime DeadLine { get; set; }

        
    }
}
