using FPT_Booking_BE.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;

namespace FPT_Booking_BE.Services
{
    public interface INotificationService
    {
        Task SendNotification(int userId, string title, string message);
        Task<IEnumerable<Notification>> GetUserNotifications(int userId);
        Task CreateNotificationAsync(Notification model);
    }
}