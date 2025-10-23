FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.sln .
COPY ProductService.Api/*.csproj ./ProductService.Api/
COPY ProductService.Application/*.csproj ./ProductService.Application/
COPY ProductService.Contract/*.csproj ./ProductService.Contract/

RUN dotnet restore "ProductService.Api/ProductService.Api.csproj"
RUN dotnet restore "ProductService.Application/ProductService.Application.csproj"
RUN dotnet restore "ProductService.Contract/ProductService.Contract.csproj"

COPY . .

WORKDIR /source/ProductService.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:5104
EXPOSE 5104
ENTRYPOINT ["dotnet", "ProductService.Api.dll"]