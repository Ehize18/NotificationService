using CSharpFunctionalExtensions;
using NotificationService.EmailService.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NotificationService.EmailService.Options;
using System.Net;
using NotificationService.Shared.Abstractions;

namespace NotificationService.EmailService.Services
{
	public class EmailSender : ISender<Email>
	{
		private readonly ILogger<EmailSender> _logger;
		private readonly SmtpOptions _smtpOptions;

		public EmailSender(ILogger<EmailSender> logger,
			IOptions<SmtpOptions> options)
		{
			_logger = logger;
			_smtpOptions = options.Value;
		}

		public async Task<Result<Email>> Send(Email email)
		{
			_logger.LogInformation($"Send message to {email.Recipient}");
			using var client = BuildClient();
			var message = new MailMessage();
			message.From = new MailAddress(_smtpOptions.Address, _smtpOptions.Name);
			message.Subject = email.Title;
			message.Body = email.Content;
			message.To.Clear();
			message.To.Add(new MailAddress(email.Recipient));
			Result<Email> result;
			try
			{
				await client.SendMailAsync(message);
				result = Result.Success(email);
				_logger.LogInformation($"Message sended to {email.Recipient}");
			}
			catch (Exception ex)
			{
				result = Result.Failure<Email>($"Fail to send message with exception {ex.Message}");
				_logger.LogError(ex, result.Error);
			}
			return result;
		}

		private SmtpClient BuildClient()
		{
			var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port);
			client.Credentials = new NetworkCredential(_smtpOptions.Name, _smtpOptions.Password);
			client.EnableSsl = true;
			return client;
		}
	}
}
