using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NotificationService.Shared.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Shared.Infrastructure
{
	internal class RabbitMQService : IRabbitMQService
	{
		private IConnection _connection;

		private IChannel _channel;

		private readonly ConnectionFactory _factory;

		private readonly ILogger<RabbitMQService> _logger;

		public RabbitMQService(string host, int port, string username, string password, ILogger<RabbitMQService> logger)
		{
			_logger = logger;

			_factory = new ConnectionFactory
			{
				HostName = host,
				Port = port,
				UserName = username,
				Password = password,
				AutomaticRecoveryEnabled = true,
				NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
			};
		}

		public async Task Connect()
		{
			try
			{
				_connection = await _factory.CreateConnectionAsync();
				_channel = await _connection.CreateChannelAsync();
				_logger.LogInformation("RabbitMQ connected");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "RabbitMQ connection fail");
				throw;
			}
		}

		public async Task PublishMessage<T>(string queueName, T message)
		{
			try
			{
				await DeclareQueue(queueName);

				var json = JsonSerializer.Serialize(message);
				var body = Encoding.UTF8.GetBytes(json);

				var properties = new BasicProperties();
				properties.Persistent = true;
				properties.ContentType = "application/json";

				await _channel.BasicPublishAsync(
					exchange: "",
					routingKey: queueName,
					mandatory: true,
					basicProperties: properties,
					body: body);

				_logger.LogInformation($"Message published to {queueName}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error publishing message to {queueName}");
				throw;
			}
		}

		public async Task Subscribe<T>(string queueName, Func<T, Task> handler)
		{
			try
			{
				await DeclareQueue(queueName);
				
				var consumer = new AsyncEventingBasicConsumer(_channel);
				consumer.ReceivedAsync += async (model, ea) =>
				{
					try
					{
						var body = ea.Body.ToArray();
						var json = Encoding.UTF8.GetString(body);
						var message = JsonSerializer.Deserialize<T>(json);
						_logger.LogInformation($"Start precess message from {queueName}");

						await handler(message!);

						await _channel.BasicAckAsync(ea.DeliveryTag, false);
						_logger.LogInformation($"Message processed from {queueName}");
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Error processing message from {queueName}");
						await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
					}
				};

				await _channel.BasicConsumeAsync(
					queue: queueName,
					autoAck: false,
					consumerTag: $"consumer-{queueName}",
					consumer: consumer);

				_logger.LogInformation($"Subscribed to {queueName}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error subscribing to {queueName}");
				throw;
			}
		}

		public async Task DeclareQueue(string queueName, bool durable = true)
		{
			try
			{
				await _channel.QueueDeclareAsync(
					queue: queueName,
					durable: durable,
					exclusive: false,
					autoDelete: false,
					arguments: null);

				///Dead letter queue.
				string dlqName = $"{queueName}.dlq";
				await _channel.QueueDeclareAsync(
					queue: dlqName,
					durable: durable,
					exclusive: false,
					autoDelete: false,
					arguments: null);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error declaring queue {queueName}");
				throw;
			}
		}

		public void Dispose()
		{
			_channel?.Dispose();
			_connection?.Dispose();
		}
	}
}
