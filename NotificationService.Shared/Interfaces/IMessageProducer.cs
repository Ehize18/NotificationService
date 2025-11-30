namespace NotificationService.Shared.Interfaces
{
	/// <summary>
	/// Message producer.
	/// </summary>
	internal interface IMessageProducer
	{
		/// <summary>
		/// Produces message.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queueName"></param>
		/// <param name="message"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task ProduceAsync<T>(string queueName, T message, CancellationToken cancellationToken = default);
	}
}
