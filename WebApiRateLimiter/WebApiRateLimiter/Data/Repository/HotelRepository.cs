using System;
using System.Collections.Generic;
using System.Linq;
using WebApiRateLimiter.Data.DatabaseContext;
using WebApiRateLimiter.Data.Interface;
using WebApiRateLimiter.Data.Model;

namespace WebApiRateLimiter.Data.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository() : base()
        {
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return AgodaDbContext.Hotels;
        }

        public IEnumerable<Hotel> GetHotelsByCity(string city)
        {
            return AgodaDbContext.Hotels.Where(x => string.Equals(city, x.City, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Hotel> GetHotelsByRoomType(string roomType)
        {
            return AgodaDbContext.Hotels.Where(x => string.Equals(roomType, x.Room, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}