using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Shared.Abstractions;
using NotificationService.Shared.Rabbit.Options;
using NotificationService.Telegram.Database;
using NotificationService.Telegram.Database.Repositories;
using NotificationService.Telegram.Entities;
using NotificationService.Telegram.Options;
using NotificationService.Telegram.Services;

namespace NotificationService.Telegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var services = new ServiceCollection()
                .Configure<TelegramBotOptions>(configuration.GetSection(nameof(TelegramBotOptions)))
				.Configure<ConsumerOptions>(configuration.GetSection(nameof(ConsumerOptions)))
				.AddLogging()
				.AddTransient<ISender<Message>, TelegramMessageSender>()
				.AddSingleton<RabbitMQConsumer>()
				.AddDbContext<TelegramDbContext>(options =>
				{
					options.UseNpgsql(configuration.GetConnectionString(nameof(TelegramDbContext)));
				}, ServiceLifetime.Transient)
				.AddTransient<MessageRepository>()
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
