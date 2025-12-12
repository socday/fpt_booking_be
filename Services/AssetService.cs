using FPT_Booking_BE.Models;
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
    }
}