
using AuthService.src.AuthService.Infrastructure.Interfaces;
using AuthService.src.AuthService.Infrastructure.Jwt;
using Microsoft.EntityFrameworkCore;
using AuthService.src.AuthService.Infrastructure.Data;
using AuthService.src.AuthService.Infrastructure.Repositories;
using AuthService.src.AuthService.Application.Services;

using AuthService.src.AuthService.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, UserAuthService>();
builder.Services.AddSingleton<JwtTokenGenerator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
