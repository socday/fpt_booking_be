namespace FPT_Booking_BE.DTOs
{
    public class UpdateConditionRequest
    {
        public int Id { get; set; }
        public string Condition { get; set; } = "Good";
        public int? Quantity { get; set; }
    }
}
