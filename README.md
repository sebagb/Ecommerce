# Ecommerce Microservices

The Ecommerce Microservices is built with ASP.NET Core Minimal APIs, integrates queue messaging with RabbitMQ, multiple database providers and delivers everything composed in docker containers.

It does not intend to prove a solution to the ecommerce business but to showcase the use of isolated APIs communicating between them, message queuing with event driven actions and docker containers for ease of installation.

## Services

- ***UserService*** allows customers to register an account needed to create orders.
- ***ProductService*** stores every detail about the product catalog.
- ***OrderService*** by communicating with the rest of services validates the request for an order.
- ***PaymentService*** processes an order's payment as a background service.

Each service implements their own repository and stores the data on databases accessible only by the responsible service.

## Endpoints

### User Service

`POST /users/` -> Creates a user account

`PUT /users/` -> Updates the user provided on a JWT with the values on the JSON body

`GET /users/{id}` -> Gets every value of a user by its id with no consideration for privacy

`GET /users/` -> Gets every value of a user by its credentials with no consideration for privacy

### Product Service

`POST /products/` -> Creates a product

`PUT /products/{id}` -> Updates a product

`GET /products/{id}` -> Gets a product by its id

### Orders Service

`POST /orders/` -> Creates an order for a `ProductId` accounted to a `UserId`. Validates the existence of the user and reduces product stock by ordered quantity. Publishes a message requiring payment processing.

`GET /orders/{id}` -> Gets an order by its id

**Payment status**: once payment is processed by the payment service a message will be published in the queue with the order id and updated status. The *Order Service* updates the order status and in case of failed payment will restock the product with the reserved quantity.

## Databases

- **User Service** -> PostgreSQL + Entity Framework
- **Product Service** -> NoSQL MongoDB
- **Orders Service** -> MySQL + Dapper
- **Payment Service** ->  In-memory XML (future implementation)

## Docker

Each service contains a dockerfile so an image can be made of them.
A docker compose file is provided at the root of the repository which creates images and containers for every service, database provider and message queueing provider.
The dockerfile at the root of the repository is needed for the *Orders Service* facilitating the inclusion of its dependencies on other projects.

## First Steps

## Getting up and running

1) `git clone https://github.com/sebagb/Ecommerce.git`
2) `docker compose up -d` at the root directory
3) `dotnet tool install --global dotnet-ef`
4) `cd UserService/UserService.Application/`
5) `dotnet ef database update`
6) Dotnet services might not run from the start if a dependency wasn't ready on time. Check container logs and restart if needed.

## A sample test flow
1) `POST http://localhost:5030/users/`
```json
{
	"username": "BobDylan",
	"password": "PlainSight",
	"email": "Bob@email.com"
}
```

2) `POST http://localhost:5104/products`
```json
{
	"Name": "Food",
	"Category": "Good",
	"Price": 20.32,
	"Provider": "CatFriends",
	"Stock": 500
}
```

3) `POST http://localhost:5018/orders/`
```json
{
	"CustomerId": "customerGuid",
	"ProductId": "productGuid",
	"Price": 20.32,
	"Quantity": 3
}
```

4) `GET http://localhost:5018/orders/{orderId}`
Payment will be `Pending` before the payment service process it and it will be either `Failed` or `Succeeded` once it does.

5 `GET http://localhost:5104/products/{productId}`
Product stock will be reduced by order quantity while order is `Pending` and it will be restocked if payment fails.

## (A few) Possible improvements:
- [ ] Automate UserService database update `dotnet ef database update`
- [ ] Add health checks to the services ensuring restart when dependencies were not ready in time.
- [ ] Constrain schemes to provide just the needed data.
- [ ] Provide detailed error messages during error `500 Internal Server Error`
- [ ] Prove functionality with unit tests