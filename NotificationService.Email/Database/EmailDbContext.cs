using Microsoft.EntityFrameworkCore;
using NotificationService.EmailService.Database.Configurations;
using NotificationService.EmailService.Database.DTO;

namespace NotificationService.EmailService.Database
{
	public class EmailDbContext : DbContext
	{
		public DbSet<EmailDTO> Email { get; set; }

		public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new EmailConfiguration());
		}
	}
}
