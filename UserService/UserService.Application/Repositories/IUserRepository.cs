using UserService.Application.Models;

namespace UserService.Application.Repositories;

public interface IUserRepository
{
    public bool Create(User user);
    public User? GetByCredentials(string username, string password);
    public User? GetById(Guid id);
    public bool Update(User user);
}