using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AuditData
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets("bf3f6e45-3dd2-44a9-acf0-4c807df0235c")
                .Build();

            var connectionString = configuration.GetSection("ConnectionStrings:AuditDBConnection").Value;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            var connectionSettings = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(connectionString);
            var connectionOptions = Options.Create(connectionSettings);
            return new DataContext(connectionSettings.Options);
        }
    }
}
