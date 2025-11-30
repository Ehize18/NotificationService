namespace NotificationService.Shared.Enums
{
	/// <summary>
	/// Statuses of messages.
	/// </summary>
	public enum MessageStatus
	{
		Pending,
		Processing,
		Sent,
		Failed,
		Retrying,
		DeadLettered
	}
}
