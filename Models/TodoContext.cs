using Microsoft.EntityFrameworkCore;

namespace crud.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }

        private const string ConnectionString = "Server=localhost;Database=todo2;Uid=root;";
        public TodoContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var serverVersion = ServerVersion.AutoDetect(ConnectionString);
            options.UseMySql(ConnectionString, serverVersion);
        }
    }
}
