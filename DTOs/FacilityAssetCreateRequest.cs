namespace FPT_Booking_BE.DTOs
{
    public class FacilityAssetCreateRequest
    {
        public int FacilityId { get; set; }
        public int AssetId { get; set; }
        public int Quantity { get; set; } = 1;
        public string Condition { get; set; } = "Good";
    }
}
