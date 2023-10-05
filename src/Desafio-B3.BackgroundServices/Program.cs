using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Application.RabbitMQ.Consumer;
using ImplementandoRabiitMQ.BackgroundServices.Configuration;
using ImplementandoRabiitMQ.BackgroundServices.Consumer;
using ImplementandoRabiitMQ.BackgroundServices.Interface;
using ImplementandoRabiitMQ.Data.Context;
using ImplementandoRabiitMQ.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Iniciando Processo de fila RabbitMQ...");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {

                services.AddDbContext<B3DbContext>(options =>
                options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                services.AddHostedService<InfoWork>();
                services.AddScoped<IConsumer, Consumer>();
                services.AddScoped<IConsumerService, ConsumerService>();
                services.AddScoped<ITarefasRepository, TarefasRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            })
         .UseSerilog((hostingContext, loggerConfiguration) =>
         {
             loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
         })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", true, true);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                config.AddJsonFile($"secrets/appsettings.{env.EnvironmentName}.json", true, true);
                config.AddEnvironmentVariables();
            });
}
