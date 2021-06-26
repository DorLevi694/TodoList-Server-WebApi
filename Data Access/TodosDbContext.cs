using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.DataModels.Entities;

namespace TodoListWebApi.Data_Access
{
    public class TodosDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }   
        
        
        public TodosDbContext(DbContextOptions<TodosDbContext> options)
            : base(options)
        {
        }



    }
}
