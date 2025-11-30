using NotificationService.Shared.Entities;

namespace NotificationService.Shared.Interfaces
{
	/// <summary>
	/// Service to send notifications.
	/// </summary>
	public interface IMessageService
	{
		/// <summary>
		/// Sends message to recipient.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <returns>Id of message.</returns>
		Task<Guid> SendMessageAsync(Message message);

		/// <summary>
		/// Gets message by id.
		/// </summary>
		/// <param name="id">Id of message.</param>
		/// <returns>Message.</returns>
		Task<Message?> GetMessageAsync(Guid id);

		/// <summary>
		/// Updates message.
		/// </summary>
		/// <param name="message">Message to update.</param>
		/// <returns>True if message updated otherwise false.</returns>
		Task<bool> UpdateMessageAsync(Message message);
	}
}
