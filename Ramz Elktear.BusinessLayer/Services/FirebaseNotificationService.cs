using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; // ✅ Import Newtonsoft.Json
using Ramz_Elktear.BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly ILogger<FirebaseNotificationService> _logger;

        public FirebaseNotificationService(ILogger<FirebaseNotificationService> logger, IConfiguration configuration)
        {
            _logger = logger;

            // Initialize Firebase Admin SDK only once
            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseConfig = configuration.GetSection("Firebase").GetChildren();
                var jsonCredentials = JsonConvert.SerializeObject(configuration.GetSection("Firebase").Get<Dictionary<string, object>>()); // ✅ Convert to JSON string

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(jsonCredentials) // ✅ Load from JSON string
                });
            }
        }

        /// <summary>
        /// Sends a notification to a Firebase topic.
        /// </summary>
        public async Task<bool> SendNotificationToTopicAsync(string topic, string title, string message)
        {
            try
            {
                var messagePayload = new Message
                {
                    Topic = topic, // ✅ Use method parameter
                    Notification = new Notification
                    {
                        Title = title,
                        Body = message
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "click_action", "FLUTTER_NOTIFICATION_CLICK" },
                        { "type", "general" }
                    },
                    Android = new AndroidConfig
                    {
                        Priority = Priority.High
                    },
                    Apns = new ApnsConfig
                    {
                        Aps = new Aps
                        {
                            ContentAvailable = true
                        }
                    }
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(messagePayload);
                _logger.LogInformation($"✅ FCM Notification sent successfully: {response}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error sending FCM Notification: {ex.Message}");
                return false;
            }
        }
    }
}
