using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetConflictingBooking(int facilityId, DateOnly date, int slotId);
        Task AddBooking(Booking booking);
        Task<IEnumerable<Booking>> GetBookings(int? userId, string? status);
        Task<Booking?> GetBookingById(int id);
        Task UpdateBooking(Booking booking);
        Task<List<int>> GetBookedSlotIds(int facilityId, DateOnly date);
        Task<bool> IsBookingConflict(int facilityId, DateOnly bookingDate, int slotId);
        Task<Booking?> GetConflictingBooking2(int facilityId, DateOnly bookingDate, int slotId);

        Task<Booking?> GetBookingByIdAsync(int id);
        Task UpdateBookingAsync(Booking booking);
    }
}