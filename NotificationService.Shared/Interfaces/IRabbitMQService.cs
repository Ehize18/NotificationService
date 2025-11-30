namespace NotificationService.Shared.Interfaces
{
	/// <summary>
	/// Service for work with RabbitMQ.
	/// </summary>
	public interface IRabbitMQService : IDisposable
	{
		/// <summary>
		/// Publish message to queue.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queueName"></param>
		/// <param name="message"></param>
		Task PublishMessage<T>(string queueName, T message);

		/// <summary>
		/// Subscribe to queue.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queueName"></param>
		/// <param name="handler"></param>
		Task Subscribe<T>(string queueName, Func<T, Task> handler);

		/// <summary>
		/// Declare Queue in RabbitMQ.
		/// </summary>
		/// <param name="queueName"></param>
		/// <param name="durable"></param>
		Task DeclareQueue(string queueName, bool durable = true);
	}
}
