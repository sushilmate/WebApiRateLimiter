namespace WebApiRateLimiter.Data.Model
{
    /// <summary>
    /// Holel Model
    /// </summary>
    public partial class Hotel
    {
        public string City { get; set; }

        public int HotelId { get; set; }

        public string Room { get; set; }

        public int Price { get; set; }
    }
}