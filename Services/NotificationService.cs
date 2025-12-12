using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;

namespace FPT_Booking_BE.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        public NotificationService(INotificationRepository repo) { _repo = repo; }

        public async Task SendNotification(int userId, string title, string message)
        {
            var noti = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now
            };
            await _repo.AddNotification(noti);
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(int userId)
        {
            return await _repo.GetMyNotifications(userId);
        }

        public async Task CreateNotificationAsync(Notification model)
        {
            if (model.CreatedAt == null)
            {
                model.CreatedAt = DateTime.Now;
            }

            await _repo.AddNotification(model);
        }
    }
}