namespace NotificationService.API.Contracts.Request
{
	public class NotificationRequest
	{
		public required string Type { get; set; }
		public required string Recipient { get; set; }
		public required string Content { get; set; }

		public Dictionary<string, object>? Metadata { get; set; }
	}
}
