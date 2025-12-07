using System;
using System.Collections.Generic;
using System.Text;
using NotificationService.Shared.Enums;

namespace NotificationService.EmailService.Database.DTO
{
	public class EmailDTO
	{
		public Guid Id { get; set; }

		public string Recipient { get; set; } = string.Empty;

		public string Title { get; set; } = string.Empty;

		public string Content { get; set; } = string.Empty;

		public MessageStatus Status { get; set; }

		public int RetryCount { get; set; }

		public Guid CreatedById { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}
}
