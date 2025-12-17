# Recurring Booking Utilities - Documentation

## Overview
This system provides comprehensive utilities for handling recurring bookings with various patterns including daily, weekly, weekdays, weekends, monthly, and custom schedules.

## Files Created

### 1. Utils/DateTimeUtils.cs
Utility functions for date operations:
- `GetVietnameseDayOfWeek(date)` - Converts date to Vietnamese day format (2=Monday, 8=Sunday)
- `GetStartOfWeek(date)` - Gets Monday of the week
- `GetEndOfWeek(date)` - Gets Sunday of the week
- `GetStartOfMonth(date)` - Gets first day of month
- `GetEndOfMonth(date)` - Gets last day of month
- `GetWeekOfMonth(date)` - Gets week number in month
- `IsWeekend(date)` / `IsWeekday(date)` - Check day type
- `GetVietnameseDayName(date)` - Gets day name in Vietnamese
- `GetDateRange(startDate, endDate)` - Gets all dates in range
- `FilterByDaysOfWeek(dates, daysOfWeek)` - Filters dates by specific days

### 2. Utils/RecurrencePattern.cs
Enum defining booking patterns:
- `Daily` - Every day
- `Weekly` - Specific days each week
- `Weekdays` - Monday to Friday only
- `Weekends` - Saturday and Sunday only
- `Monthly` - Same date each month
- `Custom` - User-defined days

### 3. Utils/RecurringBookingUtils.cs
Core logic for recurring bookings:
- `GenerateRecurringDates(request)` - Generates all booking dates
- `ValidateRecurringRequest(request)` - Validates the request
- `GetRecurrenceDescription(request)` - Human-readable description
- `GetEstimatedBookingCount(request)` - Counts bookings to be created

### 4. DTOs/BookingRecurringRequest.cs (Updated)
Enhanced with new properties:
- `Pattern` - RecurrencePattern enum
- `Interval` - Repetition interval (e.g., every 2 weeks)
- `AutoFindAlternative` - Auto-find alternative rooms
- `SkipConflicts` - Skip conflicts or fail entirely

## Usage Examples

### Example 1: Daily Bookings
Book a room every day for a week:
```json
{
  "facilityId": 1,
  "slotId": 2,
  "purpose": "Daily training session",
  "startDate": "2025-12-17",
  "endDate": "2025-12-24",
  "pattern": 1,
  "interval": 1,
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

### Example 2: Weekly Bookings (Every Monday and Wednesday)
```json
{
  "facilityId": 1,
  "slotId": 3,
  "purpose": "Weekly lecture",
  "startDate": "2025-12-17",
  "endDate": "2026-03-17",
  "pattern": 2,
  "daysOfWeek": [2, 4],
  "interval": 1,
  "autoFindAlternative": true,
  "skipConflicts": true
}
```
Note: DaysOfWeek uses Vietnamese format: 2=Monday, 3=Tuesday, 4=Wednesday, 5=Thursday, 6=Friday, 7=Saturday, 8=Sunday

### Example 3: Weekdays Only
Book Monday through Friday:
```json
{
  "facilityId": 5,
  "slotId": 1,
  "purpose": "Weekday study sessions",
  "startDate": "2025-12-17",
  "endDate": "2025-12-31",
  "pattern": 3,
  "interval": 1,
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

### Example 4: Weekends Only
Book Saturday and Sunday:
```json
{
  "facilityId": 10,
  "slotId": 4,
  "purpose": "Weekend sports practice",
  "startDate": "2025-12-21",
  "endDate": "2026-01-31",
  "pattern": 4,
  "interval": 1,
  "autoFindAlternative": false,
  "skipConflicts": true
}
```

### Example 5: Bi-Weekly (Every 2 Weeks)
Book every other week on Tuesdays:
```json
{
  "facilityId": 3,
  "slotId": 2,
  "purpose": "Bi-weekly meeting",
  "startDate": "2025-12-17",
  "endDate": "2026-06-17",
  "pattern": 2,
  "daysOfWeek": [3],
  "interval": 2,
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

### Example 6: Monthly Bookings
Book the same date each month:
```json
{
  "facilityId": 7,
  "slotId": 5,
  "purpose": "Monthly review meeting",
  "startDate": "2025-12-17",
  "endDate": "2026-12-17",
  "pattern": 5,
  "interval": 1,
  "autoFindAlternative": true,
  "skipConflicts": true
}
```

### Example 7: Custom Pattern (Tuesday, Thursday, Saturday)
```json
{
  "facilityId": 2,
  "slotId": 3,
  "purpose": "Custom schedule training",
  "startDate": "2025-12-17",
  "endDate": "2026-02-17",
  "pattern": 6,
  "daysOfWeek": [3, 5, 7],
  "interval": 1,
  "autoFindAlternative": true,
  "skipConflicts": false
}
```

## Key Features

### 1. Automatic Conflict Resolution
- **Priority Override**: Higher priority users can override lower priority bookings
- **Alternative Rooms**: Automatically finds alternative rooms if original is booked
- **Conflict Skipping**: Can skip conflicts and continue or fail entirely

### 2. Flexible Patterns
- **Daily**: Every N days
- **Weekly**: Specific days each N weeks
- **Weekdays/Weekends**: Convenient presets
- **Monthly**: Same date each N months
- **Custom**: Any combination of days

### 3. Validation
- Start date must be before end date
- Start date cannot be in the past
- Interval must be positive
- Days of week must be valid (2-8)
- Maximum 1 year range

### 4. Detailed Logging
Each booking attempt is logged with:
- Date and day name
- Success/failure status
- Reason (booked original room, found alternative, conflict, etc.)

## Response Format

```json
{
  "success": true,
  "message": "Hoàn tất đặt phòng định kỳ. Thành công: 45/50 buổi.",
  "recurrenceId": "guid-here",
  "recurrencePattern": "Hàng tuần vào Thứ Hai, Thứ Tư",
  "totalAttempted": 50,
  "successCount": 45,
  "failedCount": 5,
  "logs": [
    "17/12/2025 (Thứ Tư): ✓ THÀNH CÔNG - Đặt đúng phòng yêu cầu",
    "19/12/2025 (Thứ Năm): ✓ THÀNH CÔNG - Phòng gốc bận, chuyển sang Room 102",
    "21/12/2025 (Thứ Bảy): ✗ THẤT BẠI - Phòng gốc bận, không có phòng thay thế"
  ]
}
```

## Code Usage in Controllers

```csharp
[HttpPost("recurring")]
public async Task<IActionResult> CreateRecurring([FromBody] BookingRecurringRequest request)
{
    var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
    var result = await _bookingService.CreateRecurringBooking(userId, request);
    return Ok(result);
}
```

## Testing with Today's Date (12/17/2025)

Today is Wednesday (12/17/2025). Example requests:

**1. Rest of this week (Wed-Sun):**
```json
{
  "facilityId": 1,
  "slotId": 2,
  "purpose": "This week sessions",
  "startDate": "2025-12-17",
  "endDate": "2025-12-21",
  "pattern": 1,
  "interval": 1
}
```

**2. Every Wednesday for next 3 months:**
```json
{
  "facilityId": 1,
  "slotId": 2,
  "purpose": "Wednesday meetings",
  "startDate": "2025-12-17",
  "endDate": "2026-03-17",
  "pattern": 2,
  "daysOfWeek": [4],
  "interval": 1
}
```

## Notes
- Vietnamese day format: 2=Mon, 3=Tue, 4=Wed, 5=Thu, 6=Fri, 7=Sat, 8=Sun
- RecurrenceGroupId links all bookings in a recurring series
- Bookings can be managed individually or as a group using RecurrenceGroupId
