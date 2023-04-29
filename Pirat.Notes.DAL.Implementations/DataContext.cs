using Microsoft.EntityFrameworkCore;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Implementations
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}