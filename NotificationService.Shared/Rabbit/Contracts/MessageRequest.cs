namespace NotificationService.Shared.Rabbit.Contracts
{
	public class MessageRequest
	{
		public required Guid Id { get; set; }
		public required Guid PublisherId { get; set; }
		public required string Recipient { get; set; }
		public string Content { get; set; } = string.Empty;

		public Dictionary<string, object>? Metadata { get; set; }
	}
}
