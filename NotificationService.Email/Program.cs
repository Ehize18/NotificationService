using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.EmailService.Entities;
using NotificationService.EmailService.Options;
using NotificationService.EmailService.Services;
using NotificationService.Shared.Abstractions;
using NotificationService.Shared.Rabbit.Options;

namespace NotificationService.EmailService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var services = new ServiceCollection()
                .Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)))
                .Configure<ConsumerOptions>(configuration.GetSection(nameof(ConsumerOptions)))
                .AddLogging()
                .AddTransient<ISender<Email>, EmailSender>()
                .AddSingleton<RabbitMQConsumer>()
                .BuildServiceProvider();

            var rabbitHost = services.GetRequiredService<RabbitMQConsumer>();

            await rabbitHost.StartAsync();

			Console.WriteLine("Press any key to exti");
            Console.Read();

            await rabbitHost.StopAsync();
        }

        private static IConfiguration BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return configuration;
        }
    }
}
