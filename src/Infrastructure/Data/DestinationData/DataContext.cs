using Microsoft.EntityFrameworkCore;

namespace DestinationData
{
    public class DataContext : DbContext
    {
        private const string SchemeName = "data_store";

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
