namespace NotificationService.Shared.Entities
{
	/// <summary>
	/// Request to send message.
	/// </summary>
	public class SendMessageRequest
	{
		/// <summary>
		/// Channel of sending.
		/// </summary>
		public required string Channel { get; set; }

		/// <summary>
		/// Body of message.
		/// </summary>
		public required string Body { get; set; }

		/// <summary>
		/// Subject of message.
		/// </summary>
		public string? Subject { get; set; }

		/// <summary>
		/// Recipient of message.
		/// </summary>
		public required string Recipient { get; set; }

		/// <summary>
		/// Metadata of message.
		/// </summary>
		public Dictionary<string, string> Metadata { get; set; } = new();

		/// <summary>
		/// Send retry count.
		/// </summary>
		public int? MaxRetries { get; set; } = 3;

		/// <summary>
		/// Message sender.
		/// </summary>
		public Guid Sender { get; set; }
	}
}
