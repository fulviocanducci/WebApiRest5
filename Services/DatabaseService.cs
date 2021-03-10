using Microsoft.EntityFrameworkCore;
using Services.Mapping;
using Shared;

namespace Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DataService : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DataService()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Todo> Todo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./db.db");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new TodoMap());
        }
    }
}