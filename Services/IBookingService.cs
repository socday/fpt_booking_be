using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IBookingService
    {
        Task<string> CreateBooking(int userId, BookingCreateRequest request);

        Task<List<BookingHistoryDto>> GetHistory(int userId);

        Task<bool> UpdateStatus(int bookingId, string status, string? rejectionReason);

        Task<List<int>> GetBookedSlots(int facilityId, DateOnly date);

        Task<string> CancelBooking(int userId, int bookingId);

        Task<List<BookingHistoryDto>> GetDailyScheduleForSecurity(int campusId);

        Task<object> CreateRecurringBooking(int userId, BookingRecurringRequest request);

        Task<string> UpdateRecurringStatus(string recurrenceId, string status);

        Task<bool> StaffCancelBookingAsync(int bookingId, int staffId, string reason);
    }
}