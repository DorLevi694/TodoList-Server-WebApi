using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.Contracts;
using TodoListWebApi.Data_Access;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.Services.Repositories
{
    public class TodosDbRepository : ITodosRepository
    {

        private readonly TodosDbContext _todosDbContext;

        public TodosDbRepository(TodosDbContext todosDbContext)
        {
            _todosDbContext = todosDbContext;
        }


        public async Task<List<TodoItem>> GetAllTodoItems()
        {
            return await _todosDbContext.TodoItems.ToListAsync();

        }
        public async Task<List<TodoList>> GetAllTodoLists(bool includeItems = false)
        {
            if (includeItems)
            {
                return await _todosDbContext.TodoLists
                                            .Include(list => list.Items)
                                            .ToListAsync();
            }
            else
            {
                return await _todosDbContext.TodoLists.ToListAsync();
            }
        }



        public async Task<TodoList> GetTodoListById(int id)
        {
            return await _todosDbContext.TodoLists.FindAsync(id);
        }
        public async Task<TodoItem> GetTodoItemById(int id)
        {
            return await _todosDbContext.TodoItems.FindAsync(id);
        }









        public async Task<TodoList> AddNewTodoList(TodoList todoList)
        {
            await _todosDbContext.TodoLists.AddAsync(todoList);
            await _todosDbContext.SaveChangesAsync();

            return todoList;
        }


        public async Task<TodoItem> AddNewTodoItem(TodoItem todoItem)
        {
            var list = await this.GetTodoListById(todoItem.ListId);

            todoItem.List = list ?? throw new ArgumentNullException
                                                ($"The TodoList(id: {todoItem.ListId}) isn't found.");

            await _todosDbContext.TodoItems.AddAsync(todoItem);
            await _todosDbContext.SaveChangesAsync();

            return todoItem;
        }








        public async Task<TodoList> UpdateTodoList(int id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                throw new FormatException($"The id in body({id} is differnt from the id in the url({todoList.Id}).");
            }

            var list = await _todosDbContext.TodoLists.FindAsync(id);
            if (list == null)
            {
                throw new ArgumentException($"The todoList id: '{id}' not found.");
            }

            list.Caption = todoList.Caption;
            list.Color = todoList.Color;
            list.Description = todoList.Description;
            list.ImageUrl = todoList.ImageUrl;
            //list.Items = todoList.Items; ;
            // _todosDbContext.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _todosDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TodoListExists(id))
                {
                    throw new ArgumentException($"The todoList id: '{id}' not found.");
                }
                else
                {
                    throw;
                }
            }
            return todoList;
        }





        public async Task<TodoItem> UpdateTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                throw new FormatException($"The id in body({id} is differnt from the id in the url({todoItem.Id}).");
            }

            var item = await _todosDbContext.TodoItems.FindAsync(id);
            if (item == null)
            {
                throw new ArgumentException($"The todoItem id: '{id}' not found.");
            }

            item.Caption = todoItem.Caption;
            item.IsCompleted = todoItem.IsCompleted;

          //  _todosDbContext.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _todosDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TodoItemExists(id))
                {
                    throw new ArgumentException($"The todoItem id: '{id}' not found.");
                }
                else
                {
                    throw;
                }
            }
            return todoItem;
        }



        /*   var item = await this.GetTodoItemById(id);
               if(item == null)
                   throw
               item.Caption = todoItem.Caption;
               item.IsCompleted = todoItem.IsCompleted;
               item.ListId = todoItem.ListId;
               item.List = todoItem.List;






           }*/

        private async Task<bool> TodoItemExists(int id)
        {
            return await _todosDbContext.TodoItems.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> TodoListExists(long id)
        {
            return await _todosDbContext.TodoLists.AnyAsync(e => e.Id == id);
        }





        public async Task<Task> DeleteTodoList(int id)
        {
            var todoList = await _todosDbContext.TodoLists.FindAsync(id);

            if (todoList == null)
                throw new ArgumentException($"The TodoList(id: {todoList.Id}) isn't found.");

            _todosDbContext.TodoLists.Remove(todoList);
            await _todosDbContext.SaveChangesAsync();

            return Task.CompletedTask;
        }
        public async Task<Task> DeleteTodoItem(int id)
        {
            var todoItem = await _todosDbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
                throw new ArgumentException($"The TodoIten(id: {todoItem.Id}) isn't found.");

            _todosDbContext.TodoItems.Remove(todoItem);
            await _todosDbContext.SaveChangesAsync();

            return Task.CompletedTask;
        }





    }
}
