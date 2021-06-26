﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.Contracts
{
    public interface ITodosItemsRepository
    {
        // Get
        Task<List<TodoItem>> GetAllTodoItems();
        Task<TodoItem> GetTodoItemById(int id);


        // Post
        Task<TodoItem> AddNewTodoItem(TodoItem todoItem);


        // put
        Task<TodoItem> UpdateTodoItem(int id, TodoItem todoItem);


        // delete
        Task<Task> DeleteTodoItem(int id);


    }
}
