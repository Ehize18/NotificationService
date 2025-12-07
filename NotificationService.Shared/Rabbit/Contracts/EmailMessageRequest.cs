using System.ComponentModel.DataAnnotations;

namespace NotificationService.Shared.Rabbit.Contracts
{
	public class EmailMessageRequest
	{
		public required Guid PublisherId { get; set; }

		[EmailAddress]
		public required string Recipient { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
	}
}
