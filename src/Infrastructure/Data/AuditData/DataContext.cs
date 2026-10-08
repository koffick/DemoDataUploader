using Microsoft.EntityFrameworkCore;

namespace AuditData
{
    public class DataContext : DbContext
    {
        private const string SchemeName = "audit_store";

        internal DbSet<Models.FileActionHistory> FileActionHistory { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemeName);

            modelBuilder
                .Entity<Models.FileActionHistory>()
                .HasIndex(x => new { x.FileId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
