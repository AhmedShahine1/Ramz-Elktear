using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class NotificationController : BaseController
    {
        private readonly IFirebaseNotificationService _notificationService;

        public NotificationController(IFirebaseNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Send a notification to a Firebase topic.
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
        {
            var response = new BaseResponse();

            try
            {
                if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Message))
                {
                    response.status = false;
                    response.ErrorCode = 400;
                    response.ErrorMessage = "Title and message are required.";
                    return BadRequest(response);
                }

                var success = await _notificationService.SendNotificationToTopicAsync("Notification", request.Title, request.Message);

                if (success)
                {
                    response.Data = "Notification sent successfully!";
                    return Ok(response);
                }

                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = "Failed to send notification.";
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
    /// <summary>
    /// DTO for sending notifications
    /// </summary>
    public class NotificationRequestDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
