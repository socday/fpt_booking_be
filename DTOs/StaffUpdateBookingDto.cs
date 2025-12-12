namespace FPT_Booking_BE.DTOs
{
    public class StaffUpdateBookingDto
    {
        public DateOnly NewDate { get; set; }
        public int NewSlotId { get; set; }
    }
}