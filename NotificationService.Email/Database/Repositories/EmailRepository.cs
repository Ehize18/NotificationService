using Microsoft.EntityFrameworkCore;
using NotificationService.EmailService.Database.DTO;

namespace NotificationService.EmailService.Database.Repositories
{
	public class EmailRepository
	{
		private readonly EmailDbContext _context;

		public EmailRepository(EmailDbContext context)
		{
			_context = context;
		}

		public async Task<EmailDTO> CreateAsync(EmailDTO emailDTO)
		{
			emailDTO.CreatedAt = DateTime.UtcNow;
			_context.Email.Add(emailDTO);
			await _context.SaveChangesAsync();
			return emailDTO;
		}

		public async Task<EmailDTO?> GetByIdAsync(Guid id)
		{
			return await _context.Email
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<EmailDTO>> GetByCreatorAsync(Guid createdById, int page = 1, int pageSize = 10)
		{
			return await _context.Email
				.Where(x => x.CreatedById == createdById)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<EmailDTO> UpdateAsync(EmailDTO emailDTO)
		{
			emailDTO.UpdatedAt = DateTime.UtcNow;
			_context.Email.Update(emailDTO);
			await _context.SaveChangesAsync();
			return emailDTO;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var email = await this.GetByIdAsync(id);
			if (email == null)
			{
				return false;
			}
			_context.Email.Remove(email);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
