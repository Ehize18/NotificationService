using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Contracts.Request;
using NotificationService.API.Services;

namespace NotificationService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly NotificationService.API.Services.NotificationService _notificationService;

		public NotificationController(NotificationService.API.Services.NotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		[HttpPost]
		public async Task<IActionResult> SendNotification([FromBody]NotificationRequest request)
		{
			var result = await _notificationService.SendNotification(request);

			if (result)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
