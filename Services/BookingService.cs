using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;

namespace FPT_Booking_BE.Services
{
    public class BookingService : IBookingService
    {
        private readonly FptFacilityBookingContext _context;
        private readonly IBookingRepository _bookingRepo;
        private readonly INotificationService _notificationService;
        private readonly ISlotRepository _slotRepo;


        public BookingService(
            IBookingRepository bookingRepo, 
            FptFacilityBookingContext context,
            INotificationService notificationService,
            ISlotRepository slotRepo
            )
        {
            _bookingRepo = bookingRepo;
            _context = context;
            _notificationService = notificationService;
            _slotRepo = slotRepo;
        }

        public async Task<string> CreateBooking(int userId, BookingCreateRequest request)
        {
            var currentUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (currentUser == null) return "User không tồn tại.";

            var facility = await _context.Facilities.Include(f => f.Type).FirstOrDefaultAsync(f => f.FacilityId == request.FacilityId);
            if (facility == null) return "Phòng không tồn tại.";

            if (GetRolePriority(currentUser.Role.RoleName) == 1) 
            {
                var allowedTypes = new List<string> { "Phòng học", "Sân banh", "Phòng Lab" };
                if (!allowedTypes.Contains(facility.Type.TypeName))
                    return "Sinh viên không có quyền đặt loại phòng này.";
            }


            var conflictBooking = await _bookingRepo.GetConflictingBooking(
                request.FacilityId, request.BookingDate, request.SlotId);

            if (conflictBooking != null)
            {
                int currentPriority = GetRolePriority(currentUser.Role.RoleName);
                int ownerPriority = GetRolePriority(conflictBooking.User.Role.RoleName);

                // Chỉ được đè nếu Quyền mình CAO HƠN (Staff > GV, Staff > SV)
                if (currentPriority > ownerPriority)
                {

                    conflictBooking.Status = "Cancelled";
                    //conflictBooking.Note = $"Đã bị hủy bởi {currentUser.Role.RoleName} để ưu tiên sự kiện trường.";

                    _context.Bookings.Update(conflictBooking);
                    await _notificationService.CreateNotificationAsync(new Notification
                    {
                        UserId = conflictBooking.UserId,
                        Title = "Lịch đặt bị hủy do ưu tiên",
                        Message = $"Đơn đặt phòng {conflictBooking.BookingId} của bạn đã bị hủy để ưu tiên cho sự kiện nhà trường.",
                        CreatedAt = DateTime.Now,
                        IsRead = false
                    });
                }
                else
                {
                    return "Phòng này đã có người đặt trong Slot này rồi (Bạn không đủ quyền ưu tiên để ghi đè)!";
                }
            }

            var newBooking = new Booking
            {
                UserId = userId,
                FacilityId = request.FacilityId,
                BookingDate = request.BookingDate,
                SlotId = request.SlotId,
                Purpose = request.Purpose,
                Status = "Pending",
                BookingType = "Individual",
                PriorityLevel = "Low",
                CreatedAt = DateTime.Now
            };

            // Nếu là cao nhất đặt đè, set luôn Approved 
            if (GetRolePriority(currentUser.Role.RoleName) == 3)
            {
                newBooking.Status = "Approved";
            }

            await _bookingRepo.AddBooking(newBooking); 
            return "Success";
        }

        private int GetRolePriority(string roleName)
        {
            var normalizedRole = roleName?.Trim().ToLower() ?? "";

            switch (normalizedRole)
            {
                // LEVEL 3: Cán bộ quản lý (Đè được GV và SV)
                case "admin":
                case "facilityadmin":
                case "manager":
                    return 3;

                // LEVEL 2: Giảng viên (Đè được SV)
                case "lecturer":
                    return 2;

                // LEVEL 1: Sinh viên (Không đè được ai, chỉ đặt khi trống)
                case "student":
                    return 1;

                // LEVEL 0: Bảo vệ hoặc Role lạ (Không có quyền ưu tiên)
                case "security":
                default:
                    return 0;
            }
        }

        public async Task<List<BookingHistoryDto>> GetHistory(int userId)
        {
            var rawData = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Select(b => new
                {
                    b.BookingId,
                    b.UserId,
                    b.BookingDate,
                    b.Status,

                    FacilityName = b.Facility.FacilityName,
                    SlotName = b.Slot.SlotName,
                    StartTime = b.Slot.StartTime,
                    EndTime = b.Slot.EndTime
                })
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            var historyDtos = rawData.Select(b => new BookingHistoryDto
            {
                Id = b.BookingId,
                UserId = b.UserId,

                BookingDate = b.BookingDate,

                Status = b.Status,
                FacilityName = b.FacilityName,
                SlotName = b.SlotName,
                
                StartTime = b.StartTime,
                EndTime = b.EndTime

            }).ToList();

            return historyDtos;
        }

        public async Task<bool> UpdateStatus(int bookingId, string status, string? rejectionReason)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return false;
            if (booking.Status != "Pending") return false;

            booking.Status = status;

            if (status == "Rejected")
            {
                booking.RejectionReason = rejectionReason;

                await _notificationService.CreateNotificationAsync(new Notification
                {
                    UserId = booking.UserId,
                    Title = "Yêu cầu đặt phòng bị từ chối",
                    Message = $"Yêu cầu số {booking.BookingId} bị từ chối. Lý do: {rejectionReason}",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                });
            }
            else if (status == "Approved")
            {
                await _notificationService.CreateNotificationAsync(new Notification
                {
                    UserId = booking.UserId,
                    Title = "Đặt phòng thành công",
                    Message = $"Yêu cầu số {booking.BookingId} đã được duyệt.",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<int>> GetBookedSlots(int facilityId, DateOnly date)
        {
            return await _bookingRepo.GetBookedSlotIds(facilityId, date);
        }

        public async Task<string> CancelBooking(int userId, int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null) return "Không tìm thấy đơn đặt phòng.";

            if (booking.UserId != userId)
            {
                return "Bạn không có quyền hủy đơn đặt phòng của người khác!";
            }

            if (booking.Status == "Cancelled") return "Đơn này đã được hủy trước đó rồi.";
            if (booking.Status == "Rejected") return "Đơn này đã bị từ chối, không cần hủy nữa.";

             if (booking.BookingDate < DateOnly.FromDateTime(DateTime.Now)) return "Không thể hủy đơn trong quá khứ.";

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<bool> StaffCancelBookingAsync(int bookingId, int staffId, string reason)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null) return false;

            booking.Status = "Cancelled";       
            booking.UpdatedBy = staffId;       

            await _bookingRepo.UpdateBookingAsync(booking);

            var noti = new Notification
            {
                UserId = booking.UserId, 
                Title = "Lịch đặt phòng bị hủy",
                Message = $"Lịch đặt phòng ngày {booking.BookingDate:dd/MM/yyyy} đã bị hủy. Lý do: {reason}",
                Type = "BookingCancelled",
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            await _notificationService.CreateNotificationAsync(noti);

            return true; 
        }

        public async Task<bool> StaffModifyBookingAsync(int bookingId, StaffUpdateBookingDto request, int staffId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) throw new Exception("Không tìm thấy lịch đặt.");

            var conflict = await _bookingRepo.GetConflictingBooking(booking.FacilityId, request.NewDate, request.NewSlotId);
            if (conflict != null && conflict.BookingId != bookingId) 
            {
                throw new Exception("Lịch mới này đã có người đặt rồi, vui lòng chọn giờ khác.");
            }

            var oldDate = booking.BookingDate;

            booking.BookingDate = request.NewDate;
            booking.SlotId = request.NewSlotId;
            booking.UpdatedBy = staffId;
            booking.UpdatedAt = DateTime.Now;
            booking.Status = "Approved";

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            await _notificationService.CreateNotificationAsync(new Notification
            {
                UserId = booking.UserId,
                Title = "Lịch đặt đã được điều chỉnh",
                Message = $"Lịch của bạn đã được Admin dời từ ngày {oldDate:dd/MM} sang ngày {request.NewDate:dd/MM}. Vui lòng kiểm tra lại.",
                CreatedAt = DateTime.Now,
                IsRead = false
            });

            return true;
        }


        public async Task<List<BookingHistoryDto>> GetDailyScheduleForSecurity(int campusId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var bookings = await _context.Bookings
                .Include(b => b.Facility)
                .Include(b => b.Slot)
                .Include(b => b.User)
                .Where(b =>
                    b.BookingDate == today &&          
                    b.Status == "Approved" &&          
                    b.Facility.CampusId == campusId    
                )
                .OrderBy(b => b.Slot.StartTime)        
                .Select(b => new BookingHistoryDto
                {
                    Id = b.BookingId,
                    UserId = b.UserId, 
                    FacilityName = b.Facility.FacilityName,
                    SlotName = b.Slot.SlotName,
                    StartTime = b.Slot.StartTime,
                    EndTime = b.Slot.EndTime,    
                    Status = b.Status,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<object> CreateRecurringBooking(int userId, BookingRecurringRequest request)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
            var originalFacility = await _context.Facilities
                .Include(f => f.Type)
                .Include(f => f.Campus)
                .FirstOrDefaultAsync(f => f.FacilityId == request.FacilityId);

            if (user == null || originalFacility == null) return "Dữ liệu không hợp lệ";

            if (GetRolePriority(user.Role.RoleName) == 1)
            {
                var allowedTypes = new List<string> { "Phòng học", "Sân banh", "Phòng Lab" };
                if (!allowedTypes.Contains(originalFacility.Type.TypeName))
                    return "Sinh viên không có quyền đặt loại phòng này.";
            }

            Guid recurrenceId = Guid.NewGuid();
            var resultLog = new List<string>();
            DateOnly currentDate = request.StartDate;
            int successCount = 0;

            while (currentDate <= request.EndDate)
            {

                int currentDayVN = (int)currentDate.DayOfWeek == 0 ? 8 : (int)currentDate.DayOfWeek + 1;

                if (request.DaysOfWeek.Contains(currentDayVN))
                {
                    string dateStr = currentDate.ToString("dd/MM/yyyy");

                    int finalFacilityId = originalFacility.FacilityId;
                    bool canBook = false;
                    string note = "";

                    var conflict = await _bookingRepo.GetConflictingBooking(originalFacility.FacilityId, currentDate, request.SlotId);

                    if (conflict == null)
                    {
                        canBook = true;
                        note = "Đặt đúng phòng yêu cầu";
                    }
                    else
                    {
                        int myPriority = GetRolePriority(user.Role.RoleName);
                        int ownerPriority = GetRolePriority(conflict.User.Role.RoleName);

                        if (myPriority > ownerPriority)
                        {
                            conflict.Status = "Cancelled";
                            _context.Bookings.Update(conflict);
                            await _notificationService.CreateNotificationAsync(new Notification
                            {
                                UserId = conflict.UserId,
                                Title = "Lịch bị hủy do ưu tiên (Định kỳ)",
                                Message = $"Lịch ngày {dateStr} của bạn bị hủy để ưu tiên.",
                                CreatedAt = DateTime.Now,
                                IsRead = false
                            });
                            canBook = true;
                            note = $"Đã đè lịch của {conflict.User.Email}";
                        }
                        else
                        {
                            var alternativeFacilities = await _context.Facilities
                                .Where(f => f.CampusId == originalFacility.CampusId &&
                                            f.TypeId == originalFacility.TypeId &&
                                            f.Status == "Available" &&
                                            f.FacilityId != originalFacility.FacilityId)
                                .ToListAsync();

                            foreach (var alt in alternativeFacilities)
                            {
                                bool isAltBusy = await _bookingRepo.IsBookingConflict(alt.FacilityId, currentDate, request.SlotId);
                                if (!isAltBusy)
                                {
                                    finalFacilityId = alt.FacilityId;
                                    canBook = true;
                                    note = $"Phòng gốc bận, chuyển sang {alt.FacilityName}";
                                    break;
                                }
                            }
                            if (!canBook) note = "THẤT BẠI: Phòng gốc bận, không có phòng thay thế.";
                        }
                    }

                    if (canBook)
                    {
                        var newBooking = new Booking
                        {
                            UserId = userId,
                            FacilityId = finalFacilityId,
                            BookingDate = currentDate,
                            SlotId = request.SlotId,
                            Purpose = request.Purpose,
                            Status = (GetRolePriority(user.Role.RoleName) == 3) ? "Approved" : "Pending",
                            BookingType = "Individual",
                            RecurrenceGroupId = recurrenceId.ToString(), 
                            PriorityLevel = "Low",
                            CreatedAt = DateTime.Now
                        };
                        await _context.Bookings.AddAsync(newBooking);
                        successCount++;
                        resultLog.Add($"Ngày {dateStr}: THÀNH CÔNG ({note})");
                    }
                    else
                    {
                        resultLog.Add($"Ngày {dateStr}: {note}");
                    }
                }
                currentDate = currentDate.AddDays(1);
            }

            await _context.SaveChangesAsync();

            return new
            {
                Message = $"Hoàn tất. Thành công {successCount} buổi.",
                Logs = resultLog
            };
        }


        public async Task<string> UpdateRecurringStatus(string recurrenceId, string status)
        {
            var bookings = await _context.Bookings
                .Where(b => b.RecurrenceGroupId == recurrenceId && b.Status != "Cancelled")
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return "Không tìm thấy nhóm đơn đặt phòng này (hoặc mã không hợp lệ).";
            }

            foreach (var booking in bookings)
            {
                booking.Status = status;

                 booking.RejectionReason = "Admin từ chối cả loạt"; 
            }

            await _context.SaveChangesAsync();

            return $"Thành công! Đã cập nhật trạng thái {status} cho {bookings.Count} đơn đặt phòng.";
        }
    }
}