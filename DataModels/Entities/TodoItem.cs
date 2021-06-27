
using System.ComponentModel.DataAnnotations;

namespace TodoListWebApi.DataModels.Entities
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Caption { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public int ListId { get; set; }
        [Required]
        public TodoList List { get; set; }
    }
}
