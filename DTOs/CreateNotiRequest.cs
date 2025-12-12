namespace FPT_Booking_BE.DTOs
{
    public class CreateNotiRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
