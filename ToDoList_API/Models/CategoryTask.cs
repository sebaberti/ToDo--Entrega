using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ToDoList_API.Models
{
    public class CategoryTask
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        [Column(Order = 0)]
        public int TaskId { get; set; }

        
        [Column(Order = 1)]
        public int CategoryId { get; set; }

        [ForeignKey("TaskId")]
        public Tasks Tasks { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Categories { get; set; }
    }
}
