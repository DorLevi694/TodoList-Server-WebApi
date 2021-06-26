using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoListWebApi.DataModels.Entities
{
    public class TodoList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Caption { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Color { get; set; }
       
        //[MinLength(30, ErrorMessage = "MinLength -at least 30 charcter.")]
        [Required]
        [RegularExpression(@".+\s.+\s.+")]//, ErrorMessage = "RegularExpression - fail at least 3 words.")]
        [StringLength(50)]

        public string Description { get; set; }

        [Required]
        public List<TodoItem> Items { get; set; }
    }
}

