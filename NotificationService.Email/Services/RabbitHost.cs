using Microsoft.Extensions.Hosting;

namespace NotificationService.EmailService.Services
{
	public class RabbitHost : IHostedService
	{
		private readonly RabbitMQConsumer _consumer;

		public RabbitHost(RabbitMQConsumer rabbitMQConsumer)
		{
			_consumer = rabbitMQConsumer;
		}
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _consumer.StartAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _consumer.StopAsync();
		}
	}
}
