using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace NotificationService.Shared.Rabbit.Clients
{
	public abstract class BaseRabbitClient
	{
		private readonly IConnectionFactory _connectionFactory;

		private IConnection? _connection;

		private IChannel? _channel;

		protected IConnection Connection
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

		protected IChannel Channel
		{
			get
			{
				if (_channel?.IsOpen != true)
					_channel = Connection.CreateChannelAsync().GetAwaiter().GetResult();

				return _channel;
			}
		}

		protected RabbitMQOptions RabbitMQOptions { get; }

		protected ILogger<BaseRabbitClient> Logger { get; }

		protected BaseRabbitClient(IOptions<RabbitMQOptions> options, ILogger<BaseRabbitClient> logger)
		{
			Logger = logger;
			RabbitMQOptions = options.Value;
			_connectionFactory = new ConnectionFactory
			{
				HostName = RabbitMQOptions.HostName,
				UserName = RabbitMQOptions.UserName,
				Password = RabbitMQOptions.Password
			};
		}
	}
}
