using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Context
{
    public class TodoDbContext :DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options):base (options)
        {

        }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

    }
}
