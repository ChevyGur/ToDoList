
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Task.Services;
using User.Interfaces;
using User.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = TokenService.GetTokenValidationParameters();
    });
// builder.Services.AddUsers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference=new OpenApiReference{Type=ReferenceType.SecurityScheme,Id="Bearer"}
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton<Task.Interfaces.ITaskService, Task.Services.TaskService>();

builder.Services.AddSingleton<User.Interfaces.IUserService, User.Services.UserService>();
// builder.Services.AddScoped

builder.Services.AddAuthorization(cfg =>
        {
            cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
            cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
        });

var app = builder.Build();
// app.UseRouter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


// OrderManagement.Extention.AddOrders();
// add scoped
namespace OrderManagement
{
    static class Extention
    {
        public static IServiceCollection AddOrders(this IServiceCollection services)
        {
            services.AddScoped<Task.Interfaces.ITaskService, Task.Services.TaskService>();
            services.AddScoped<User.Interfaces.IUserService, User.Services.UserService>();
            return services;
        }
    }
}

