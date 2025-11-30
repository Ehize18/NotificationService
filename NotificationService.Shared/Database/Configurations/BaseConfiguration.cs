using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Shared.Entities;

namespace NotificationService.Shared.Database.Configurations
{
	public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<T> builder)
		{
			builder.HasKey(x => x.Id);
		}
	}
}
