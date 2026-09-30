using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace UserData
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private const string ConfigurationFileName = "appsettings.json";

        public DataContext CreateDbContext(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigurationFileName)
                .Build();
            Console.WriteLine(Directory.GetCurrentDirectory());
            var connectionString = configuration.GetSection("ConnectionStrings:UserDBConnection").Value;
            Console.WriteLine(connectionString);

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
