using NotificationService.Shared.Enums;

namespace NotificationService.Shared.Entities
{
	/// <summary>
	/// Message
	/// </summary>
	public class Message : BaseEntity
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
		public int RetryCount { get; set; } = 0;

		/// <summary>
		/// Status of sending.
		/// </summary>
		public MessageStatus Status { get; set; }
	}
}
