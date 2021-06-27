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

        [Required]
        public string Description { get; set; }

        [Required]
        public List<TodoItem> Items { get; set; }
    }
}

