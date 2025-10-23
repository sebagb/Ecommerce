using UserService.Application.Database;
using UserService.Application.Models;

namespace UserService.Application.Repositories;

public class UserRepository(RepositoryDbContext context) : IUserRepository
{
    private readonly RepositoryDbContext context = context;

    public bool Create(User user)
    {
        var exists = context.User.Any(x => x.Username.Equals(user.Username));
        if (exists)
        {
            return false;
        }
        context.User.Add(user);
        return context.SaveChanges() > 0;
    }

    public User? GetByCredentials(string username, string password)
    {
        return context.User.SingleOrDefault(x =>
            x.Username.Equals(username) && x.Password.Equals(password));
    }

    public User? GetById(Guid id)
    {
        return context.User.SingleOrDefault(x => x.Id == id);
    }

    public bool Update(User user)
    {
        var existingUser = context.User.Any(x => x.Id == user.Id);

        if (!existingUser)
        {
            return false;
        }

        var isDuplicated = context.User.Any(x =>
            !x.Id.Equals(user.Id)
            && x.Username.Equals(user.Username));

        if (isDuplicated)
        {
            return false;
        }
        context.User.Update(user);

        return context.SaveChanges() > 0;
    }
}