
using System;
using Microsoft.Extensions.Logging;
using NotificationService.Shared.Entities;
using NotificationService.Shared.Interfaces;

namespace NotificationService.Shared.Infrastructure
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository _repository;
		private readonly ILogger<MessageService> _logger;

		public MessageService(IMessageRepository repository,
			ILogger<MessageService> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public async Task<Guid> SendMessageAsync(Message message)
		{
			await _repository.AddAsync(message);
			return message.Id;
		}

		public async Task<Message?> GetMessageAsync(Guid id)
		{
			return await _repository.GetByIdAsync(id);
		}


		public async Task<bool> UpdateMessageAsync(Message message)
		{
			var msg = await _repository.GetByIdAsync(message.Id, true);
			if (msg == null) return false;

			msg.Status = message.Status;

			return await _repository.UpdateAsync(msg);
		}
	}
}
