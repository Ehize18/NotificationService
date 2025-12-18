using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Shared.Abstractions;
using NotificationService.Telegram.Entities;
using NotificationService.Telegram.Options;
using Telegram.Bot;
using TgBotTypes = Telegram.Bot.Types;

namespace NotificationService.Telegram.Services
{
	public class TelegramMessageSender : ISender<Message>
	{
		private readonly ILogger<TelegramMessageSender> _logger;
		private readonly TelegramBotOptions _botOptions;

		public TelegramMessageSender(ILogger<TelegramMessageSender> logger,
			IOptions<TelegramBotOptions> botOptions)
		{
			_logger = logger;
			_botOptions = botOptions.Value;
		}

		public async Task<Result<Message>> Send(Message message)
		{
			_logger.LogInformation($"Send message to {message.Recipient}");
			var client = BuildClient();
			TgBotTypes.ChatId chatId;
			Result<Message> result;
			if (long.TryParse(message.Recipient, out long longId))
			{
				chatId = new TgBotTypes.ChatId(longId);
			}
			else if (message.Recipient.StartsWith("@"))
			{
				chatId = new TgBotTypes.ChatId(message.Recipient);
			}
			else
			{
				result = Result.Failure<Message>($"Fail to send message to recipient {message.Recipient}, recipient incorrect format");
				_logger.LogError(result.Error);
				return result;
			}
			try
			{
				await client.SendMessage(chatId, message.Content);
				result = Result.Success(message);
				_logger.LogInformation($"Message sended to {message.Recipient}");
			}
			catch (Exception ex)
			{
				result = Result.Failure<Message>($"Fail to send message with exception {ex.Message}");
				_logger.LogError(ex, result.Error);
			}
			return result;
		}

		private TelegramBotClient BuildClient()
		{
			var client = new TelegramBotClient(_botOptions.Token);
			return client;
		}
	}
}
