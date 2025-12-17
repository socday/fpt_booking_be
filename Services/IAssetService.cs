using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<FacilityAsset>> GetAssetsByFacilityAsync(int facilityId);

        Task<bool> UpdateAssetConditionAsync(int id, string condition, int? quantity);

        Task<(bool success, string message, FacilityAsset? asset)> CreateFacilityAssetAsync(FacilityAssetCreateRequest request);

        Task<bool> UpdateQuantityAsync(int id, int quantity);
    }
}