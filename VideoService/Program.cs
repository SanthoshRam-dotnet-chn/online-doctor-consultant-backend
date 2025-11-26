using Microsoft.EntityFrameworkCore;
using VideoService.Application;
using VideoService.Infrastructure;
using VideoService.Services;
using VideoService.Services.Providers;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IVideoProvider, JitsiProvider>(); // default provider
builder.Services.AddScoped<IVideoService, VideoServiceImpl>();
builder.Services.AddSingleton<IJoinTokenService, JoinTokenService>();

// controllers
builder.Services.AddControllers();
// swagger optional
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
