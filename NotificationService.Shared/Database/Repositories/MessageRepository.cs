using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotificationService.Shared.Entities;
using NotificationService.Shared.Interfaces;

namespace NotificationService.Shared.Database.Repositories
{
	public class MessageRepository : IMessageRepository
	{
		private readonly MessageDbContext _context;
		private readonly ILogger<MessageRepository> _logger;

		public MessageRepository(MessageDbContext context,
			ILogger<MessageRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task AddAsync(Message message)
		{
			try
			{
				_context.Messages.Add(message);
				await _context.SaveChangesAsync();
				_logger.LogInformation($"Notification {message.Id} added to database");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding notification");
				throw;
			}
		}

		public async Task<Message?> GetByIdAsync(Guid id, bool isTrack = false)
		{
			var set = _context.Messages;

			IQueryable<Message> query;

			if (!isTrack)
			{
				query = set.AsNoTracking();
			} 
			else
			{
				query = set.AsQueryable();
			}

			return await query
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<List<Message>> GetByFilterAsync(Expression<Func<Message, bool>> predicate, bool isTrack = false)
		{
			var set = _context.Messages;

			IQueryable<Message> query;

			if (!isTrack)
			{
				query = set.AsNoTracking();
			}
			else
			{
				query = set.AsQueryable();
			}

			return await query
				.Where(predicate)
				.ToListAsync();
		}

		public async Task<bool> UpdateAsync(Message message)
		{
			try
			{
				_context.Messages.Update(message);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating notification");
				return false;
			}
		}

		public async Task<bool> SaveChangesAsync()
		{
			try
			{
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error saving changes");
				return false;
			}
		}
	}
}
