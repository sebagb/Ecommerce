using System.Data;

namespace OrderService.Application.Database;

public interface IDbConnectionFactory
{
    public IDbConnection CreateConnection();
}