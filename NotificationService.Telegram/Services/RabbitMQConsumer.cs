using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Shared.Abstractions;
using NotificationService.Shared.Enums;
using NotificationService.Shared.Rabbit.Clients;
using NotificationService.Shared.Rabbit.Contracts;
using NotificationService.Shared.Rabbit.Options;
using NotificationService.Telegram.Database;
using NotificationService.Telegram.Database.DTO;
using NotificationService.Telegram.Database.Repositories;
using NotificationService.Telegram.Entities;

namespace NotificationService.Telegram.Services
{
	public class RabbitMQConsumer : BaseConsumer<MessageRequest>
	{
		public RabbitMQConsumer(IOptions<ConsumerOptions> consumerOptions,
			ILogger<BaseConsumer<MessageRequest>> logger,
			IServiceProvider serviceProvider)
			: base(consumerOptions, logger, serviceProvider)
		{
		}

		protected override async Task<bool> ProcessMessage(MessageRequest body, IServiceScope scope)
		{
			Result<Message> result;
			using (var dbContext = scope.ServiceProvider.GetRequiredService<TelegramDbContext>())
			{
				var repository = new MessageRepository(dbContext);
				var messageDTO = await repository.GetByIdAsync(body.Id).ConfigureAwait(false);
				if (messageDTO == null)
				{
					messageDTO = new MessageDTO
					{
						Id = body.Id,
						Content = body.Content,
						Recipient = body.Recipient,
						RetryCount = 0,
						CreatedById = body.PublisherId,
						Status = MessageStatus.Processing
					};
					await repository.CreateAsync(messageDTO);
				}
				else
				{
					if (messageDTO.RetryCount >= 3)
					{
						messageDTO.Status = MessageStatus.Fail;
						await repository.UpdateAsync(messageDTO);
						return true;
					}
					else
					{
						messageDTO.Status = MessageStatus.Retry;
						messageDTO.RetryCount += 1;
						await repository.UpdateAsync(messageDTO);
					}
				}
				var sendService = scope.ServiceProvider.GetRequiredService<ISender<Message>>();
				var message = new Message
				{
					Content = body.Content,
					Recipient = body.Recipient
				};
				result = await sendService.Send(message);

				if (result.IsSuccess)
				{
					messageDTO.Status = MessageStatus.Sended;
					await repository.UpdateAsync(messageDTO);
				}
			}
			

			return result.IsSuccess;
		}

		public async Task StartAsync()
		{
			await ExecuteAsync(CancellationToken.None);
		}

		public async Task StopAsync()
		{
			await DisposeAsync();
		}
	}
}
