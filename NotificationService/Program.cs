using NotificationService.Services;

using NotificationService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Email service
builder.Services.AddSingleton<IEmailService, EmailService>();

// Notification services
builder.Services.AddSingleton<RegistrationNotificationService>();
builder.Services.AddSingleton<MeetingConfirmedService>();
builder.Services.AddSingleton<MeetingCancelledService>();
builder.Services.AddSingleton<MeetingEndedService>();
builder.Services.AddSingleton<IEmailService, EmailService>();


builder.Services.AddLogging(); // Important for ILogger


var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
