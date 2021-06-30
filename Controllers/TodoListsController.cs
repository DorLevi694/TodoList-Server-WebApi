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
    public class TodoListsController : ControllerBase
    {
        private readonly ITodosRepository _todosRepository;

        public TodoListsController(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }


        [HttpGet("", Name = nameof(GetAllTodoLists))]
        public async Task<ActionResult<List<TodoListDto>>> GetAllTodoLists()
        {
            var results = (await _todosRepository.GetAllTodoLists())
                                                 .Select(list => TodoListMapper.ToTodoListDto(list));

            return Ok(results);
        }



        [HttpGet("count", Name = nameof(GetCountOfLists))]
        public async Task<ActionResult<int>> GetCountOfLists()
        {
            var count = await _todosRepository.GetCountOfLists();

            return Ok(count);
        }





        [HttpGet("{id}", Name = nameof(GetTodoListById))]
        public async Task<ActionResult<TodoListDto>> GetTodoListById(int id)
        {
            var result = await _todosRepository.GetTodoListById(id);

            if (result == null)
            {
                return NotFound();
            }

            var retVal = TodoListMapper.ToTodoListDto(result);
            return Ok(retVal);
        }


        [HttpGet("{id}/items", Name = nameof(GetAllItemsOfTodoListByListId))]
        public async Task<ActionResult<List<TodoItemDto>>> GetAllItemsOfTodoListByListId(int id)
        {
            var result = (await _todosRepository.GetAllItemsOfTodoListByListId(id))
                            .Select(item => TodoItemMapper.ToTodoItemDto(item));

            return Ok(result);
        }




        [HttpPost("", Name = nameof(AddNewTodoList))]
        public async Task<ActionResult<TodoListDto>> AddNewTodoList([FromBody] TodoListDto todoListDto)
        {
            var newList = TodoListMapper.ToTodoList(todoListDto);

            var result = await _todosRepository.AddNewTodoList(newList);

            var retVal = TodoListMapper.ToTodoListDto(result);
            return Ok(retVal);
        }





        [HttpDelete("{id}", Name = nameof(DeleteTodoList))]
        public async Task<ActionResult> DeleteTodoList(int id)
        {
            try
            {
                await _todosRepository.DeleteTodoList(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }








        [HttpPut("{id}", Name = nameof(UpdateTodoList))]
        public async Task<ActionResult<TodoListDto>> UpdateTodoList
            (int id, [FromBody] TodoListDto todoListDto)
        {
            try
            {
                var todoListToUpdate = TodoListMapper.ToTodoList(todoListDto);

                var result = await _todosRepository.UpdateTodoList(id, todoListToUpdate);

                var retVal = TodoListMapper.ToTodoListDto(result);
                return Ok(retVal);
            }
            catch (ArgumentException)
            {
                return NotFound();

            }
            catch (Exception)
            {
                return BadRequest();
            }


        }




    }





}

