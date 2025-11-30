using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NotificationService.Shared.Database.Configurations;
using NotificationService.Shared.Entities;

namespace NotificationService.Shared.Database
{
	public class MessageDbContext : DbContext
	{
		public DbSet<Message> Messages { get; set; }

		public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new MessageConfiguration());
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is BaseEntity entity)
				{
					if (entry.State == EntityState.Modified)
					{
						entity.UpdatedAt = DateTime.UtcNow;
					}
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}

