namespace NotificationService.Shared.Entities
{
	/// <summary>
	/// Config of the channel.
	/// </summary>
	public class ChannelConfig
	{
		/// <summary>
		/// Max retry count.
		/// </summary>
		public int MaxRetries { get; set; } = 3;

		/// <summary>
		/// Retry delay in seconds.
		/// </summary>
		public int RetryDelaySeconds { get; set; } = 5;

		public required Dictionary<string, string> Options { get; set; }
	}
}
