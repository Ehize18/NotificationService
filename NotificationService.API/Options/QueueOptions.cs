namespace NotificationService.API.Options
{
	public class QueueOptions
	{
		public class QueueBind
		{
			public required string QueueName { get; set; }
			public required string RoutingKey { get; set; }
		}

		public required string ExchangeName { get; set; }

		public required List<QueueBind> QueueBinds { get; set; }
	}
}
