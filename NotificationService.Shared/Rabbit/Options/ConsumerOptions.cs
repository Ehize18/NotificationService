namespace NotificationService.Shared.Rabbit.Options
{
	public class ConsumerOptions : RabbitMQOptions
	{
		public string QueueName { get; set; } = string.Empty;
	}
}
