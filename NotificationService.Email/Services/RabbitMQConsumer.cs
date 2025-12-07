using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.EmailService.Database.DTO;
using NotificationService.EmailService.Database.Repositories;
using NotificationService.EmailService.Entities;
using NotificationService.Shared.Abstractions;
using NotificationService.Shared.Enums;
using NotificationService.Shared.Rabbit.Clients;
using NotificationService.Shared.Rabbit.Contracts;
using NotificationService.Shared.Rabbit.Options;

namespace NotificationService.EmailService.Services
{
	public class RabbitMQConsumer : BaseConsumer<EmailMessageRequest>
	{
		public RabbitMQConsumer(IOptions<ConsumerOptions> consumerOptions,
			ILogger<BaseConsumer<EmailMessageRequest>> logger,
			IServiceProvider serviceProvider)
			: base(consumerOptions, logger, serviceProvider)
		{
		}

		protected override async Task<bool> ProcessMessage(EmailMessageRequest body, IServiceScope scope)
		{
			var repository = scope.ServiceProvider.GetRequiredService<EmailRepository>();
			var emailDTO = await repository.GetByIdAsync(body.Id);
			if (emailDTO == null)
			{
				emailDTO = new EmailDTO
				{
					Id = body.Id,
					Title = body.Title,
					Content = body.Content,
					Recipient = body.Recipient,
					RetryCount = 0,
					CreatedById = body.PublisherId,
					Status = MessageStatus.Processing
				};
				await repository.CreateAsync(emailDTO);
			}
			else
			{
				if (emailDTO.RetryCount >= 3)
				{
					emailDTO.Status = MessageStatus.Fail;
					await repository.UpdateAsync(emailDTO);
					return true;
				}
				else
				{
					emailDTO.Status = MessageStatus.Retry;
					emailDTO.RetryCount += 1;
					await repository.UpdateAsync(emailDTO);
				}
			}
			var sendService = scope.ServiceProvider.GetRequiredService<ISender<Email>>();
			var email = new Email
			{
				Title = body.Title,
				Content = body.Content,
				Recipient = body.Recipient
			};
			var result = await sendService.Send(email);

			if (result.IsSuccess)
			{
				emailDTO.Status = MessageStatus.Sended;
				await repository.UpdateAsync(emailDTO);
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
