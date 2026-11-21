using DestinationData.Models;
using Microsoft.EntityFrameworkCore;

namespace DestinationData
{
    public class DataContext : DbContext
    {
        private const string SchemeName = "data_store";

        internal DbSet<Field> Fields { get; set; }
        internal DbSet<Organization> Organizations { get; set; }
        internal DbSet<Well> Wells { get; set; }
        internal DbSet<TechMode> TechModes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemeName);

            base.OnModelCreating(modelBuilder);
        }
    }
}
