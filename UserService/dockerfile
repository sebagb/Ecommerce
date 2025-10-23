FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.sln .
COPY UserService.Api/*.csproj ./UserService.Api/
COPY UserService.Application/*.csproj ./UserService.Application/
COPY UserService.Contract/*.csproj ./UserService.Contract/

RUN dotnet restore "UserService.Api/UserService.Api.csproj"
RUN dotnet restore "UserService.Application/UserService.Application.csproj"
RUN dotnet restore "UserService.Contract/UserService.Contract.csproj"

COPY . .

WORKDIR /source/UserService.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:5030
EXPOSE 5030
ENTRYPOINT ["dotnet", "UserService.Api.dll"]