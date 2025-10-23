using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserService.Api;
using UserService.Api.Auth;
using UserService.Application;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["Database:ConnectionString"]!;
builder.Services.AddOpenApi();
builder.Services.AddApplication(connectionString);

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            var key = AuthConstants.TokenSecret;
            var issuer = AuthConstants.Issuer;
            var audience = AuthConstants.Audience;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true
            };
        });

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.RegisterUserEndpoints();

app.Run();