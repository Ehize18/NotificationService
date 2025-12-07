using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Shared.Rabbit.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Shared.Rabbit.Clients
{
	public abstract class BaseConsumer<T> : BackgroundService, IAsyncDisposable where T : class
	{
		private readonly IConnectionFactory _connectionFactory;
		private readonly ConsumerOptions _consumerOptions;
		private readonly ILogger<BaseConsumer<T>> _logger;
		private readonly IServiceProvider _serviceProvider;

		private IChannel? _channel;
		private IConnection? _connection;

		private IConnection Connection
		{
			get
			{
				if (_connection?.IsOpen != true)
				{
					_connection = _connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
				}
				return _connection;
			}
		}

		private IChannel Channel
		{
			get
			{
				if (_channel?.IsOpen != true)
				{
					_channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();
				}
				return _channel;
			}
		}

		protected BaseConsumer(IOptions<ConsumerOptions> consumerOptions, ILogger<BaseConsumer<T>> logger, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;

			_consumerOptions = consumerOptions.Value;
			_connectionFactory = new ConnectionFactory
			{
				HostName = _consumerOptions.HostName,
				UserName = _consumerOptions.UserName,
				Password = _consumerOptions.Password
			};
		}

		

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			return StartConsuming(_consumerOptions.QueueName, stoppingToken);
		}

		private async Task StartConsuming(string queueName, CancellationToken cancellationToken)
		{
			await Channel.QueueDeclareAsync(queueName, false, false, false, cancellationToken: cancellationToken);

			var consumer = new AsyncEventingBasicConsumer(Channel);

			consumer.ReceivedAsync += async (_, ea) =>
			{
				var bytes = ea.Body.ToArray();

				var processedSuccessfully = false;
				try
				{
					processedSuccessfully = await ProcessMessageInternal(bytes);
				}
				catch (Exception ex)
				{
					_logger.LogError($"Exception occurred while processing message from queue {queueName}: {ex}");
				}

				if (processedSuccessfully)
					await Channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
				else
					await Channel.BasicRejectAsync(ea.DeliveryTag, true, cancellationToken);
			};

			await Channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken);
		}

		private Task<bool> ProcessMessageInternal(byte[] bytes)
		{
			var bodyStr = Encoding.UTF8.GetString(bytes);
			var body = JsonSerializer.Deserialize<T>(bodyStr);
			if (body is null)
			{
				return Task.FromResult(false);
			}

			using var scope = _serviceProvider.CreateScope();

			return ProcessMessage(body, scope);
		}

		protected abstract Task<bool> ProcessMessage(T body, IServiceScope scope);

		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore();
			GC.SuppressFinalize(this);
		}

		protected virtual async ValueTask DisposeAsyncCore()
		{
			if (_channel != null)
			{
				await _channel.DisposeAsync();
			}
			if (_connection != null)
			{
				await _connection.DisposeAsync();
			}
		}
	}
}
