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


        [HttpGet("{id}", Name = nameof(GetTodoListById))]
        //  [Route()]
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







        [HttpPost("", Name = nameof(AddNewTodoList))]
        //[Route("")]
        public async Task<ActionResult<TodoListDto>> AddNewTodoList([FromBody] TodoListDto todoListDto)
        {
            var newList = TodoListMapper.ToTodoList(todoListDto);

            var result = await _todosRepository.AddNewTodoList(newList);

            var retVal = TodoListMapper.ToTodoListDto(result);
            return Ok(retVal);
        }





        [HttpDelete("{id}", Name = nameof(DeleteTodoList))]
        //[Route("{id}")]
        public async Task<ActionResult> DeleteTodoList(int id)
        {
            try
            {
                await _todosRepository.DeleteTodoList(id);
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








        [HttpPut("{id}", Name = nameof(UpdateTodoList))]
        // [Route("{id}")]
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
            catch (FormatException e)
            {
                return BadRequest("UpdateTodoList:\n" + e.Message);
            }
            catch (ArgumentException e)
            {
                return NotFound("UpdateTodoList:\n" + e.Message);

            }
            catch (Exception e)
            {
                return BadRequest("UpdateTodoList:\n" + e.Message);
            }


        }




    }





}

