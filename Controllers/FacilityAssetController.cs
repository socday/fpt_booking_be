using Microsoft.AspNetCore.Mvc;
using FPT_Booking_BE.Services;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityAssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public FacilityAssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("facility/{facilityId}")]
        public async Task<IActionResult> GetByFacility(int facilityId)
        {
            var data = await _assetService.GetAssetsByFacilityAsync(facilityId);

            return Ok(data);
        }

        [HttpPut("update-condition")]
        public async Task<IActionResult> UpdateCondition([FromBody] UpdateConditionRequest request)
        {
            var result = await _assetService.UpdateAssetConditionAsync(
                request.Id,
                request.Condition,
                request.Quantity
            );

            if (!result) return NotFound(new { message = "Không tìm thấy thiết bị này trong phòng." });

            return Ok(new { message = "Cập nhật trạng thái thiết bị thành công!" });
        }
    }

    
}