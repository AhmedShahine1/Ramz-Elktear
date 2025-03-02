using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using System.Threading.Tasks;

namespace Ramz_Elktear.Controllers.MVC
{
    public class NotificationController : Controller
    {
        private readonly IFirebaseNotificationService _notificationService;

        public NotificationController(IFirebaseNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult Send()
        {
            return View();
        }

        /// <summary>
        /// Send a notification to a Firebase topic
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Send(string title, string message)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
            {
                ViewBag.Error = "All fields are required!";
                return View();
            }

            var success = await _notificationService.SendNotificationToTopicAsync("Notification", title, message);
            ViewBag.Success = success ? "Notification sent successfully!" : "Failed to send notification.";

            return View();
        }
    }
}
