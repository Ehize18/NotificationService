using Microsoft.Extensions.Options;
using NotificationService.API.Contracts.Request;
using NotificationService.API.Options;
using NotificationService.Shared.Rabbit.Clients;
using NotificationService.Shared.Rabbit.Contracts;

namespace NotificationService.API.Services
{
	public class NotificationService
	{
		private readonly AdvancedPublishClient _client;
		private readonly QueueOptions _options;

		public NotificationService(AdvancedPublishClient client, IOptions<QueueOptions> options)
		{
			_client = client;
			_options = options.Value;
		}

		public async Task<bool> SendNotification(NotificationRequest request)
		{
			var messageRequest = new MessageRequest
			{
				Id = Guid.NewGuid(),
				PublisherId = Guid.NewGuid(),
				Recipient = request.Recipient,
				Content = request.Content,
				Metadata = request.Metadata
			};
			try
			{
				await _client.Publish(messageRequest, _options.ExchangeName, request.Type.ToLower());
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
