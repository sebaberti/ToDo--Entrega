using System.ComponentModel.DataAnnotations;

namespace ToDoList_API.Models.Dto
{
    public class UpdateTaskDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Completed { get; set; }

        public DateTime? DeadLine { get; set; }

    }
}
