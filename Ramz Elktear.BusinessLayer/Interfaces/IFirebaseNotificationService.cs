using Ramz_Elktear.core.DTO.NotificationModels;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{

    public interface IFirebaseNotificationService
    {
        Task<bool> SendNotificationToTopicAsync(string topic, string title, string message);
    }
}
