using Dapper;

namespace OrderService.Application.Database;

public class DbInitializer(IDbConnectionFactory connectionFactory)
{
    private readonly IDbConnectionFactory connectionFactory = connectionFactory;

    public void Initialize()
    {
        using var connection = connectionFactory.CreateConnection();

        connection.Execute("""
        CREATE TABLE IF NOT EXISTS Orders (
            OrderId CHAR(36) PRIMARY KEY,
            CustomerId CHAR(36),
            ProductId CHAR(36),
            OrderedOn DATETIME,
            Price DECIMAL(7,2),
            Quantity INTEGER,
            Status VARCHAR(100)
        )
        """);
    }
}