using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.EmailService.Entities;
using NotificationService.Shared.Abstractions;
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
			var sendService = scope.ServiceProvider.GetRequiredService<ISender<Email>>();
			var email = new Email
			{
				Title = body.Title,
				Content = body.Content,
				Recipient = body.Recipient
			};
			var result = await sendService.Send(email);
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
