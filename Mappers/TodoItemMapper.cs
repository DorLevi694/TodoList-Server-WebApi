using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.DataModels.Dtos;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.DataModels.Mappers
{
    public class TodoItemMapper
    {
        public static TodoItemDto ToTodoItemDto(TodoItem todoItem)
        {
            var todoItemDto = new TodoItemDto()
            {
                Id = todoItem.Id,
                Caption = todoItem.Caption,
                IsCompleted = todoItem.IsCompleted,
                ListId = todoItem.ListId,
            };

            return todoItemDto;
        }


        public static TodoItem ToTodoItem(TodoItemDto todoItemDto)
        {
            var todoItem = new TodoItem()
            {
                Id = todoItemDto.Id,
                Caption = todoItemDto.Caption,
                IsCompleted = todoItemDto.IsCompleted,
                ListId = todoItemDto.ListId,
                List = null // the repository should handle it 
            };

            return todoItem;
        }

    }
}
