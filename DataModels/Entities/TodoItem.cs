
using System.ComponentModel.DataAnnotations;

namespace TodoListWebApi.DataModels.Entities
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

       // [RegularExpression(@".+\s.+\s.+", ErrorMessage = "RegularExpression - fail at least 3 words.")]
        [Required]
        [MinLength(10)]//, ErrorMessage = "MinLength -at least 10 charcter.")]
        public string Caption { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public int ListId { get; set; }
        [Required]
        public TodoList List { get; set; }
    }
}
