namespace WebApiRateLimiter
{
    public static class LoggingMessages
    {
        public const string GetHotelByCity = "Get Hotel By City {cityName}";
        public const string GetHotelByCityWithoutParameter = "Get Hotel By City without parameter";
        public const string GetHotelByCityOrderByPrice = "Get Hotel By City {cityName} & order by price {orderByPriceAsc}";
        public const string GetHotelByRoom = "Get Hotel By Room {roomType}";
        public const string GetHotelByRoomWithoutParameter = "Get Hotel By Room without parameter";
        public const string GetHotelByRoomOrderByPrice = "Get Hotel By Room {roomType} and order by price {orderByPriceAsc}";
        public const string Forbidden = "Request has been forbidden for User ";
    }
}