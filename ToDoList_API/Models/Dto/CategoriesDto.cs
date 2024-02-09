﻿using System.ComponentModel.DataAnnotations;

namespace ToDoList_API.Models.Dto
{
    public class CategoriesDto
    {

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
