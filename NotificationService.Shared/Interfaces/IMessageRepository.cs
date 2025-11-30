using System.Linq.Expressions;
using NotificationService.Shared.Entities;
using NotificationService.Shared.Enums;

namespace NotificationService.Shared.Interfaces
{
	/// <summary>
	/// Repository of messages.
	/// </summary>
	public interface IMessageRepository
	{
		/// <summary>
		/// Adds message to repository.
		/// </summary>
		/// <param name="message">Message.</param>
		Task AddAsync(Message message);

		/// <summary>
		/// Gets message by id.
		/// </summary>
		/// <param name="id">Id of message.</param>
		/// <returns><see cref="Message"/>.</returns>
		Task<Message?> GetByIdAsync(Guid id, bool isTrack = false);

		/// <summary>
		/// Gets messages by status.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		Task<List<Message>> GetByFilterAsync(Expression<Func<Message, bool>> predicate, bool isTrack = false);

		/// <summary>
		/// Updates message in database.
		/// </summary>
		/// <param name="message">Message to update.</param>
		/// <returns>True if message updated otherwise false.</returns>
		Task<bool> UpdateAsync(Message message);

		/// <summary>
		/// Save changes.
		/// </summary>
		Task<bool> SaveChangesAsync();
	}
}
