namespace FPT_Booking_BE.DTOs
{
    public class StaffCancelRequest
    {
        public int StaffId { get; set; }
        public string Reason { get; set; } = "";
    }
}
