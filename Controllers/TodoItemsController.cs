using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.Contracts;
using TodoListWebApi.DataModels.Dtos;
using TodoListWebApi.DataModels.Entities;
using TodoListWebApi.DataModels.Mappers;
using TodoListWebApi.Services.Repositories;

namespace TodoListWebApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        
        private readonly ITodosRepository _todosRepository;

        public TodoItemsController(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }



        [HttpGet("", Name = nameof(GetAllTodoItems))]
        public async Task<ActionResult<List<TodoItemDto>>> GetAllTodoItems()
        {
            var results = (await _todosRepository.GetAllTodoItems())
                                                 .Select(item => TodoItemMapper.ToTodoItemDto(item));

            return Ok(results);
        }

        [HttpGet("ActiveItems", Name = nameof(GetAllActiveItems))]
        public async Task<ActionResult<List<TodoItemDto>>> GetAllActiveItems()
        {
            var results = (await _todosRepository.GetAllActiveItems())
                                                 .Select(item => TodoItemMapper.ToTodoItemDto(item));

            return Ok(results);
        }


        [HttpGet("count", Name = nameof(GetCountOfItems))]
        public async Task<ActionResult<int>> GetCountOfItems()
        {
            var count = await _todosRepository.GetCountOfItems();

            return Ok(count);
        }

        [HttpGet("ActiveItems/count", Name = nameof(GetCountOfActiveItems))]
        public async Task<ActionResult<int>> GetCountOfActiveItems()
        {
            var count = await _todosRepository.GetCountOfActiveItems();

            return Ok(count);
        }

        [HttpGet("{id}", Name = nameof(GetTodoItemById))]
     //   [Route("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemById(int id)
        {
            var result = await _todosRepository.GetTodoItemById(id);

            if (result == null)
            {
                return NotFound();
            }

            var retVal = TodoItemMapper.ToTodoItemDto(result);
            return Ok(retVal);
        }



        [HttpPost("",Name = nameof(AddNewTodoItem))]
        //[Route("")]
        public async Task<ActionResult<TodoItemDto>> AddNewTodoItem([FromBody] TodoItemDto todoItemDto)
        {
            try
            {
                var newItem = TodoItemMapper.ToTodoItem(todoItemDto);                

                var result = await _todosRepository.AddNewTodoItem(newItem);

                var retVal = TodoItemMapper.ToTodoItemDto(result);
                return Ok(retVal);
            }
            catch(ArgumentNullException e)
            {
                return NotFound(e.Message); //the item list isn't found
            }
        }


        [HttpDelete("{id}",Name = nameof(DeleteTodoItem))]
        //[Route()]
        public async Task<ActionResult> DeleteTodoItem(int id)
        {
            try
            {
                await _todosRepository.DeleteTodoItem(id);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }



        [HttpPut("{id}", Name = nameof(UpdateTodoItem))]
        //[Route("{id}")]
        public async Task<ActionResult<TodoItemDto>> UpdateTodoItem
            (int id, [FromBody] TodoItemDto todoItemDto)
        {
            try
            {
                var todoItemToUpdate = TodoItemMapper.ToTodoItem(todoItemDto);

                var result = await _todosRepository.UpdateTodoItem(id, todoItemToUpdate);

                var retVal = TodoItemMapper.ToTodoItemDto(result);
                return Ok(retVal);
            }
            catch (FormatException e)
            {
                return BadRequest("UpdateTodoItem:\n" + e.Message);
            }
            catch (ArgumentException e)
            {
                return NotFound("UpdateTodoItem:\n" + e.Message);

            }
            catch (Exception e)
            {
                return BadRequest("UpdateTodoItem:\n" + e.Message);
            }


        }


        private static async Task<List<TodoItem>> GetFakeTodoItemAsync()
        {
            var todoItem_1 = new TodoItem()
            {
                Id = 1,
                Caption = "Caption-1",
                IsCompleted = true,
                ListId = 1,
                List = null
            };

            var todoItem_2 = new TodoItem()
            {
                Id = 2,
                Caption = "Caption-2",
                IsCompleted = false,
                ListId = 2,
                List = null
            };

            // Create a list of parts.
            List<TodoItem> items = new () { todoItem_1, todoItem_2 };

            await Task.Delay(500);
            return items;
        }
    }
}