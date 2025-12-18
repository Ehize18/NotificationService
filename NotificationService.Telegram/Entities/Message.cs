using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Telegram.Entities
{
	public class Message
	{
		public string Recipient { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
	}
}
