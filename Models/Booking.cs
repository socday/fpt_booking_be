using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPT_Booking_BE.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int FacilityId { get; set; }

    public DateOnly BookingDate { get; set; }

    public int SlotId { get; set; }

    public string? Purpose { get; set; }

    public string? BookingType { get; set; }

    public string? Status { get; set; }

    public string? PriorityLevel { get; set; }

    public int? ApproverId { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? RecurrenceGroupId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? Approver { get; set; }

    public virtual Facility Facility { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Slot Slot { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public int? UpdatedBy { get; set; }

    [ForeignKey("UpdatedBy")]
    public virtual User? UpdatedByUser { get; set; }
}
