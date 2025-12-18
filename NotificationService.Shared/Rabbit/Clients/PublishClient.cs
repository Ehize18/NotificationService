using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Shared.Rabbit.Options;
using RabbitMQ.Client;

namespace NotificationService.Shared.Rabbit.Clients
{
	public class PublishClient : BaseRabbitClient
	{
		public PublishClient(IOptions<RabbitMQOptions> options, ILogger<BaseRabbitClient> logger) : base(options, logger)
		{
		}

		public async Task ExchangeDeclareAsync(string exchange, string type = ExchangeType.Direct)
		{
			await Channel.ExchangeDeclareAsync(exchange, type);
		}

		public async Task QueueDeclareAsync(string queue)
		{
			await Channel.QueueDeclareAsync(queue, false, false, false);
		}

		public async Task QueueBindAsync(string queue, string exchange, string routingKey)
		{
			await Channel.QueueBindAsync(queue, exchange, routingKey);
		}

		public async Task Publish<TMessage>(TMessage message, string exchange, string routingKey) where TMessage : class
		{
			var messageBytes = Encoding.UTF8.GetBytes(
				JsonSerializer.Serialize(message));
			Logger.LogInformation("Publish message");
			await Channel.BasicPublishAsync(exchange, routingKey, messageBytes);
			Logger.LogInformation("Message published");
		}
	}
}
