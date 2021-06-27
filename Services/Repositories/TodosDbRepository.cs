using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.Contracts;
using TodoListWebApi.Data_Access;
using TodoListWebApi.DataModels.Entities;
using TodoListWebApi.Utilities;




namespace TodoListWebApi.Services.Repositories
{

    public class TodosDbRepository : ITodosRepository
    {

        private readonly TodosDbContext _todosDbContext;
        public readonly int MIN_LENGTH_LIST_DESCRIPTION = 30;
        public readonly int MIN_WORDS_COUNT_LIST_DESCRIPTION = 10;
        public readonly int MIN_LENGTH_ITEM_CAPTION = 10;
        public readonly int MIN_WORDS_COUNT_ITEM_CAPTION = 3;

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
            ValidateTodoListFields(todoList);
            await _todosDbContext.TodoLists.AddAsync(todoList);
            await _todosDbContext.SaveChangesAsync();

            return todoList;
        }


        public async Task<TodoItem> AddNewTodoItem(TodoItem todoItem)
        {

            ValidateTodoItemFields(todoItem);

            var list = await this.GetTodoListById(todoItem.ListId);

            todoItem.List = list ?? throw new ArgumentNullException
                                                ($"The TodoList(id: {todoItem.ListId}) isn't found.");


            await _todosDbContext.TodoItems.AddAsync(todoItem);
            await _todosDbContext.SaveChangesAsync();

            return todoItem;
        }








        public async Task<TodoList> UpdateTodoList(int id, TodoList todoList)
        {
            ValidateTodoListFields(todoList);

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
            ValidateTodoItemFields(todoItem);

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


        // new 

        public async Task<int> GetCountOfLists()
        {
            var x = await _todosDbContext.TodoLists.CountAsync();
            return x;

        }

        public async Task<int> GetCountOfItems()
        {
            var x = await _todosDbContext.TodoItems.CountAsync();
            return x;
        }

        public async Task<int> GetCountOfActiveItems()
        {
            var x = await _todosDbContext.TodoItems
                                         .Where(item => item.IsCompleted == false)
                                         .CountAsync();

            return x;
        }

        public async Task<List<TodoItem>> GetAllItemsOfTodoListByListId(int listId)
        {
            var x = await _todosDbContext.TodoItems
                                        .Where(item => item.ListId == listId)
                                         .ToListAsync();

            return x;
        }

        public async Task<List<TodoItem>> GetAllActiveItems()
        {
            var x = await _todosDbContext.TodoItems
                                         .Where(item => item.IsCompleted == false)
                                         .ToListAsync();

            return x;
        }




        private void ValidateTodoListFields(TodoList todoList)
        {
            if (!todoList.Description.ValidateLengthAndWords(MIN_LENGTH_LIST_DESCRIPTION
                                                          , MIN_WORDS_COUNT_LIST_DESCRIPTION))
            {
                throw new Exception("Dor");
                // This request does not came from my angular app so I don't want to let him any information
            }
        }

        private void ValidateTodoItemFields(TodoItem todoItem)
        {
            if (!todoItem.Caption.ValidateLengthAndWords(MIN_LENGTH_ITEM_CAPTION
                                                          , MIN_WORDS_COUNT_ITEM_CAPTION))
            {
                throw new Exception();
                // This request does not came from my angular app so I don't want to let him any information
            }
        }


    }
}
