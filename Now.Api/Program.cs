var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<Now.Domain.Repositories.ITasksRepository, Now.Infrastructure.Repositories.TasksRepository>(t =>
{
    string connectionString = builder.Configuration.GetConnectionString("SQLiteDb") ?? "";
    return new Now.Infrastructure.Repositories.TasksRepository(connectionString);
});

builder.Services.AddTransient<Now.Application.Services.TasksService>();

builder.Services.AddControllers();
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
