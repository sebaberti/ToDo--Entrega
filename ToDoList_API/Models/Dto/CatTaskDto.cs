using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList_API.Models.Dto
{
    public class CatTaskDto
    {
        public int TaskId { get; set; }


        
        public int CategoryId { get; set; }

        [Required]
        public Tasks Tasks { get; set; }

        [Required]
        public Categories Categories { get; set; }
    }
}
