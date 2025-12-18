using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Telegram.Database.DTO;

namespace NotificationService.Telegram.Database.Configurations
{
	public class MessageConfiguration : IEntityTypeConfiguration<MessageDTO>
	{
		public void Configure(EntityTypeBuilder<MessageDTO> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasIndex(x => x.CreatedById);
		}
	}
}
