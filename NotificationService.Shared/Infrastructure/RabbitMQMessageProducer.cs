using Microsoft.Extensions.Logging;
using NotificationService.Shared.Interfaces;

namespace NotificationService.Shared.Infrastructure
{
	public class RabbitMQMessageProducer : IMessageProducer
	{
		private readonly IRabbitMQService _rabbitMqService;
		private readonly ILogger<RabbitMQMessageProducer> _logger;

		public RabbitMQMessageProducer(IRabbitMQService rabbitMqService,
			ILogger<RabbitMQMessageProducer> logger)
		{
			_rabbitMqService = rabbitMqService;
			_logger = logger;
		}

		public async Task ProduceAsync<T>(string queueName, T message,
			CancellationToken cancellationToken = default)
		{
			await Task.Run(() =>
			{
				try
				{
					_rabbitMqService.PublishMessage(queueName, message);
					_logger.LogInformation($"Message produced to queue {queueName}");
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Error producing message to {queueName}");
					throw;
				}
			}, cancellationToken);
		}
	}
}
