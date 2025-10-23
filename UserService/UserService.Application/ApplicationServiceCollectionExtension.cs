using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Database;
using UserService.Application.Repositories;

namespace UserService.Application;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection service, string connectionString)
    {
        service.AddDbContext<RepositoryDbContext>(options =>
            options.UseNpgsql(connectionString));

        service.AddScoped<IUserRepository, UserRepository>();
        return service;
    }
}