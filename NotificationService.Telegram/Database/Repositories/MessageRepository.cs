using Microsoft.EntityFrameworkCore;
using NotificationService.Telegram.Database.DTO;

namespace NotificationService.Telegram.Database.Repositories
{
	public class MessageRepository
	{
		private readonly TelegramDbContext _context;

		public MessageRepository(TelegramDbContext context)
		{
			_context = context;
		}

		public async Task<MessageDTO> CreateAsync(MessageDTO messageDTO)
		{
			messageDTO.CreatedAt = DateTime.UtcNow;
			_context.Message.Add(messageDTO);
			await _context.SaveChangesAsync();
			return messageDTO;
		}

		public async Task<MessageDTO?> GetByIdAsync(Guid id)
		{
			return await _context.Message
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<MessageDTO>> GetByCreatorAsync(Guid createdById, int page = 1, int pageSize = 10)
		{
			return await _context.Message
				.Where(x => x.CreatedById == createdById)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<MessageDTO> UpdateAsync(MessageDTO messageDTO)
		{
			messageDTO.UpdatedAt = DateTime.UtcNow;
			_context.Message.Update(messageDTO);
			await _context.SaveChangesAsync();
			return messageDTO;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var message = await this.GetByIdAsync(id);
			if (message == null)
			{
				return false;
			}
			_context.Message.Remove(message);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
