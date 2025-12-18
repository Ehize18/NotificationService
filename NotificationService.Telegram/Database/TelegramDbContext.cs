using Microsoft.EntityFrameworkCore;
using NotificationService.Telegram.Database.Configurations;
using NotificationService.Telegram.Database.DTO;

namespace NotificationService.Telegram.Database
{
	public class TelegramDbContext : DbContext
	{
		public DbSet<MessageDTO> Message { get; set; }

		public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new MessageConfiguration());
		}
	}
}
