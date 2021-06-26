using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.Contracts
{
    public interface ITodosListsRepository
    {
        // get
        Task<List<TodoList>> GetAllTodoLists(bool includeItems=false);
        Task<TodoList> GetTodoListById(int id);


        // post
        Task<TodoList> AddNewTodoList(TodoList todoList);


        // put
        Task<TodoList> UpdateTodoList(int id, TodoList todoList);


        //delete
        Task<Task> DeleteTodoList(int id);

    }
}
