using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Shared.Entities;

namespace NotificationService.Shared.Database.Configurations
{
	public class MessageConfiguration : BaseConfiguration<Message>
	{
		public override void Configure(EntityTypeBuilder<Message> builder)
		{
			base.Configure(builder);

			builder.HasIndex(x => x.Status)
				.IsUnique(false);

			builder.HasIndex(x => x.CreatedBy)
				.IsUnique(false);
		}
	}
}
