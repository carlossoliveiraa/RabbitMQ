using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Application.RabbitMQ.Consumer;
using ImplementandoRabiitMQ.Application.RabbitMQ.Producer;
using ImplementandoRabiitMQ.Data.Context;
using ImplementandoRabiitMQ.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();


builder.Services.AddDbContext<B3DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//para aceitar as solicitacoes do angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Substitua pelo domínio do seu aplicativo Angular
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddScoped<B3DbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITarefasRepository, TarefasRepository>();
builder.Services.AddScoped<IProducer, Producer>();
builder.Services.AddScoped<IConsumer, Consumer>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//para aceitar solicitacoes do angular
app.UseCors("AllowSpecificOrigin");


app.UseHttpsRedirection();

app.UseAuthorization();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Certifique-se de que o nível seja adequado
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

app.MapControllers();

app.Run();