FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY OrderService/*.sln ./OrderService/
COPY OrderService/OrderService.Api/*.csproj ./OrderService/OrderService.Api/
COPY OrderService/OrderService.Application/*.csproj ./OrderService/OrderService.Application/
COPY OrderService/OrderService.Contract/*.csproj ./OrderService/OrderService.Contract/

COPY Ecommerce.UserService/ ./UserService/
COPY ProductService/ ./ProductService/

RUN dotnet restore "OrderService/OrderService.Api/OrderService.Api.csproj"
RUN dotnet restore "OrderService/OrderService.Application/OrderService.Application.csproj"
RUN dotnet restore "OrderService/OrderService.Contract/OrderService.Contract.csproj"

COPY . .

WORKDIR /source/OrderService/OrderService.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:5018
EXPOSE 5018
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]