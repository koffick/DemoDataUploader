using Microsoft.EntityFrameworkCore;

namespace UserData
{
    public class DataContext : DbContext
    {
        private const string SchemeName = "user_store";

        internal DbSet<Models.User> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemeName);

            modelBuilder
                .Entity<Models.User>()
                .HasIndex(x => new { x.LoginName })
                .IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
