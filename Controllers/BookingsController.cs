using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq; 
using System.Security.Claims;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly INotificationService _notiService;

        public BookingsController(IBookingService bookingService, INotificationService notiService)
        {
            _bookingService = bookingService;
            _notiService = notiService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
        {
            var userIdClaim = User.FindFirst("UserId"); 
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ hoặc thiếu thông tin UserID" });
            }

            int userId = int.Parse(userIdClaim.Value);

            var result = await _bookingService.CreateBooking(userId, request);

            if (result != "Success")
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { message = "Gửi yêu cầu đặt phòng thành công!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory([FromQuery] string? status)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var bookings = await _bookingService.GetHistory(userId);

            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status.ToLower() == status.ToLower()).ToList();
            }

            return Ok(bookings);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Manager")] 
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatusUpdate request)
        {
            if (request.Status != "Approved" && request.Status != "Rejected")
            {
                return BadRequest(new { message = "Trạng thái không hợp lệ. Chỉ chấp nhận 'Approved' hoặc 'Rejected'." });
            }

            var result = await _bookingService.UpdateStatus(id, request.Status, request.RejectionReason);

            if (!result)
            {
                return BadRequest(new { message = "Không tìm thấy đơn đặt phòng hoặc đơn này không ở trạng thái 'Pending' để duyệt." });
            }

            return Ok(new { message = $"Đã cập nhật trạng thái thành {request.Status}" });
        }


        [HttpGet("availability")]
        public async Task<IActionResult> CheckAvailability([FromQuery] int facilityId, [FromQuery] DateOnly date)
        {
            var bookedSlots = await _bookingService.GetBookedSlots(facilityId, date);

            return Ok(new
            {
                facilityId = facilityId,
                date = date,
                bookedSlotIds = bookedSlots
            });
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var result = await _bookingService.CancelBooking(userId, id);

            if (result == "Success")
            {
                return Ok(new { message = "Hủy đặt phòng thành công!" });
            }
            else if (result.Contains("quyền")) 
            {
                return Forbid(result);
            }
            else
            {
                return BadRequest(new { message = result });
            }
        }

        [HttpGet("schedule-today")]
        //[Authorize(Roles = "Security,Admin,Manager")]
        public async Task<IActionResult> GetSecuritySchedule([FromQuery] int campusId)
        {
            var schedule = await _bookingService.GetDailyScheduleForSecurity(campusId);
            return Ok(schedule);
        }


        [HttpPost("recurring")]
        public async Task<IActionResult> CreateRecurring([FromBody] BookingRecurringRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            if (request.EndDate < request.StartDate)
                return BadRequest(new { message = "Ngày kết thúc phải lớn hơn ngày bắt đầu" });

            var result = await _bookingService.CreateRecurringBooking(userId, request);

            return Ok(result);
        }

        [HttpPut("recurring/{recurrenceId}/status")]
        public async Task<IActionResult> UpdateRecurringStatus(string recurrenceId, [FromBody] BookingStatusUpdate request)
        {
            if (request.Status != "Approved" && request.Status != "Rejected")
            {
                return BadRequest(new { message = "Trạng thái không hợp lệ. Chỉ chấp nhận 'Approved' hoặc 'Rejected'." });
            }

            var result = await _bookingService.UpdateRecurringStatus(recurrenceId, request.Status);

            if (result.Contains("Không tìm thấy"))
            {
                return NotFound(new { message = result });
            }

            return Ok(new { message = result });
        }


        [HttpPut("staff-cancel/{id}")]
        public async Task<IActionResult> StaffCancelBooking(int id, [FromBody] StaffCancelRequest request)
        {
            var result = await _bookingService.StaffCancelBookingAsync(id, request.StaffId, request.Reason);

            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy lịch đặt phòng này." });
            }

            return Ok(new { message = "Đã hủy lịch và gửi thông báo thành công!" });
        }

    }
}   