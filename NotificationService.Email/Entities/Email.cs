namespace NotificationService.EmailService.Entities
{
	public class Email
	{
		public string Recipient { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
	}
}
