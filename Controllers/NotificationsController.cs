using FPT_Booking_BE.Models;
using FPT_Booking_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;
        public NotificationsController(INotificationService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            return Ok(await _service.GetUserNotifications(userId));
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotiRequest request)
        {
            var noti = new Notification
            {
                UserId = request.UserId,
                Title = request.Title,
                Message = request.Message,
                Type = "System",
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            await _service.CreateNotificationAsync(noti);

            return Ok(new { message = "Đã tạo thông báo thành công!" });
        }
    }
}
