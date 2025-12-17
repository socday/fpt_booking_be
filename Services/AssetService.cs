using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class AssetService : IAssetService
    {
        private readonly FptFacilityBookingContext _context;

        public AssetService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FacilityAsset>> GetAssetsByFacilityAsync(int facilityId)
        {
            return await _context.FacilityAssets
                .Include(fa => fa.Asset)  
                                            
                .Where(fa => fa.FacilityId == facilityId)
                .ToListAsync();
        }

        public async Task<bool> UpdateAssetConditionAsync(int id, string condition, int? quantity)
        {
            var asset = await _context.FacilityAssets.FindAsync(id);
            if (asset == null) return false;

            asset.Condition = condition;

            if (quantity.HasValue)
            {
                asset.Quantity = quantity.Value;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool success, string message, FacilityAsset? asset)> CreateFacilityAssetAsync(FacilityAssetCreateRequest request)
        {
            // Validate facility exists
            var facility = await _context.Facilities.FindAsync(request.FacilityId);
            if (facility == null)
            {
                return (false, "Phòng không tồn tại.", null);
            }

            // Validate asset exists
            var asset = await _context.Assets.FindAsync(request.AssetId);
            if (asset == null)
            {
                return (false, "Tài sản không tồn tại.", null);
            }

            // Check if asset already assigned to this facility
            var existingAssignment = await _context.FacilityAssets
                .FirstOrDefaultAsync(fa => fa.FacilityId == request.FacilityId && fa.AssetId == request.AssetId);

            if (existingAssignment != null)
            {
                return (false, $"Tài sản '{asset.AssetName}' đã được gán cho phòng này rồi. Sử dụng update-quantity để thay đổi số lượng.", null);
            }

            // Validate quantity
            if (request.Quantity < 1)
            {
                return (false, "Số lượng phải lớn hơn 0.", null);
            }

            // Create new facility asset
            var facilityAsset = new FacilityAsset
            {
                FacilityId = request.FacilityId,
                AssetId = request.AssetId,
                Quantity = request.Quantity,
                Condition = request.Condition ?? "Good"
            };

            await _context.FacilityAssets.AddAsync(facilityAsset);
            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(facilityAsset).Reference(fa => fa.Asset).LoadAsync();
            await _context.Entry(facilityAsset).Reference(fa => fa.Facility).LoadAsync();

            return (true, "Tạo tài sản cho phòng thành công!", facilityAsset);
        }

        public async Task<bool> UpdateQuantityAsync(int id, int quantity)
        {
            var facilityAsset = await _context.FacilityAssets.FindAsync(id);
            if (facilityAsset == null) return false;

            if (quantity < 0)
            {
                throw new ArgumentException("Số lượng không thể âm.");
            }

            facilityAsset.Quantity = quantity;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}