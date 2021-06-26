using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.DataModels.Dtos;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.DataModels.Mappers
{
    public class TodoListMapper
    {
        public static TodoListDto ToTodoListDto(TodoList todoList)
        {
            var todoListDto = new TodoListDto()
            {
                Id = todoList.Id,
                Caption = todoList.Caption,
                Color = todoList.Color,
                Description = todoList.Description,
                ImageUrl = todoList.ImageUrl
            };

            return todoListDto;
        }

        public static TodoList ToTodoList(TodoListDto todoListDto)
        {

            var todoList = new TodoList()
            {
                Id = todoListDto.Id,
                Caption = todoListDto.Caption,
                Color = todoListDto.Color,
                Description = todoListDto.Description,
                ImageUrl = todoListDto.ImageUrl,
                Items = new List<TodoItem>()
            };
            return todoList;
        }
    }

}
