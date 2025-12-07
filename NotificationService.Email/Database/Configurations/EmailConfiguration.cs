using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.EmailService.Database.DTO;

namespace NotificationService.EmailService.Database.Configurations
{
	public class EmailConfiguration : IEntityTypeConfiguration<EmailDTO>
	{
		public void Configure(EntityTypeBuilder<EmailDTO> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasIndex(x => x.CreatedById);
		}
	}
}
