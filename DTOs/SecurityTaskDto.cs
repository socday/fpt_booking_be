namespace FPT_Booking_BE.DTOs
{
    public class SecurityTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Normal";
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ReportNote { get; set; }
        public int? BookingId { get; set; }
        public int? SlotId { get; set; }
        public string? FacilityName { get; set; }
    }
}
