namespace WebApiRateLimiter
{
    public static class Constants
    {
        public const string HOTELS_BY_CITY_WEBAPI_URL = "api/city/bangkok";

        public const string HOTELS_BY_ROOM_WEBAPI_URL = "api/room/deluxe";

        public const string FORBIDDEN_CONTENT = "Web API Rate Limit Exceeded.";

        public const string SUSPEND_CONTENT = "Web API is suspended, Please try after sometime.";

        public const string THROTTLE_BASE_KEY = "ThrottleBaseKey";
    }
}
