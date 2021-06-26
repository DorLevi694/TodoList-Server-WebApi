using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.Contracts
{
    public interface ITodosRepository : ITodosListsRepository, ITodosItemsRepository { };
}
