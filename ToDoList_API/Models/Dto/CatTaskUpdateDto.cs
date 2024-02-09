using System.ComponentModel.DataAnnotations;

namespace ToDoList_API.Models.Dto
{
    public class CatTaskUpdateDto
    {
        public int TaskId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Tasks Tasks { get; set; }

        [Required]
        public Categories Categories { get; set; }


    }
}
