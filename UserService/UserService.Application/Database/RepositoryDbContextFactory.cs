using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserService.Application.Database;

internal class RepositoryDbContextFactory
    : IDesignTimeDbContextFactory<RepositoryDbContext>
{
    public RepositoryDbContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var appSettings = $"appsettings.{env}.json";
        var config = new ConfigurationBuilder()
            .AddJsonFile(appSettings)
            .Build();

        var connection = config.GetSection("Database:ConnectionString").Value;

        var optionsBuilder = new DbContextOptionsBuilder<RepositoryDbContext>();
        optionsBuilder.UseNpgsql(connection);

        return new RepositoryDbContext(optionsBuilder.Options);
    }
}