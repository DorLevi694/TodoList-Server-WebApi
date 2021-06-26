using System;
namespace TodoListWebApi.DataModels.Dtos
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public bool IsCompleted { get; set; }
        public int ListId { get; set; }
    }
}
