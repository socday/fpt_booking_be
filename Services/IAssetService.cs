using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<FacilityAsset>> GetAssetsByFacilityAsync(int facilityId);

        Task<bool> UpdateAssetConditionAsync(int id, string condition, int? quantity);
    }
}