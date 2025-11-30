using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Shared.Database;
using NotificationService.Shared.Database.Repositories;
using NotificationService.Shared.Interfaces;

namespace NotificationService.Shared.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMessageServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString(nameof(MessageDbContext));

			services.AddDbContext<MessageDbContext>(options =>
			{
				options.UseNpgsql(connectionString);
			});

			services.AddScoped<IMessageRepository, MessageRepository>();

			services.AddScoped<IMessageService, Messa>
		}
	}
}
