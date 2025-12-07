namespace NotificationService.Shared.Rabbit
{
	public class ConsumerOptions : RabbitMQOptions
	{
		public string QueueName { get; set; } = string.Empty;
	}
}
