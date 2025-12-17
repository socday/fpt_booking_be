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

        [HttpPost("create")]
        public async Task<IActionResult> CreateFacilityAsset([FromBody] FacilityAssetCreateRequest request)
        {
            var (success, message, asset) = await _assetService.CreateFacilityAssetAsync(request);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new 
            { 
                message, 
                data = asset 
            });
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            try
            {
                var result = await _assetService.UpdateQuantityAsync(request.Id, request.Quantity);

                if (!result)
                {
                    return NotFound(new { message = "Không tìm thấy tài sản này trong phòng." });
                }

                return Ok(new { message = "Cập nhật số lượng thành công!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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