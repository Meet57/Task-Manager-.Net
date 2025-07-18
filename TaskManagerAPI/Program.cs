using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services;
using TaskManagerAPI.Services.Interfaces;

var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = envName,
});

// Configure Kestrel BEFORE building the app
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Environment: {envName}");
Console.WriteLine($"Connection String: {connectionString}");

// Add DbContext with retry on failure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(5))
);

// Register DI
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();