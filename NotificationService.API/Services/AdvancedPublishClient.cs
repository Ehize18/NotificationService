using Microsoft.Extensions.Options;
using NotificationService.API.Options;
using NotificationService.Shared.Rabbit.Clients;
using NotificationService.Shared.Rabbit.Options;

namespace NotificationService.API.Services
{
	public class AdvancedPublishClient : PublishClient
	{
		private readonly QueueOptions _queueOptions;
		public AdvancedPublishClient(IOptions<RabbitMQOptions> rabbitOptions,
			IOptions<QueueOptions> queueOptions,
			ILogger<BaseRabbitClient> logger) : base(rabbitOptions, logger)
		{
			_queueOptions = queueOptions.Value;
			Initialize().GetAwaiter().GetResult();
		}

		private async Task Initialize()
		{
			await ExchangeDeclareAsync(_queueOptions.ExchangeName);

			foreach (var queue in _queueOptions.QueueBinds)
			{
				await QueueDeclareAsync(queue.QueueName);
				await QueueBindAsync(queue.QueueName, _queueOptions.ExchangeName, queue.RoutingKey);
			}
		}
	}
}
