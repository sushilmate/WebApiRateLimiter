using WebApiRateLimiter.Data.DatabaseContext;
using WebApiRateLimiter.Data.Interface;
using WebApiRateLimiter.Data.Model;
using System.Collections.Generic;
using System.Linq;

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
            return AgodaDbContext.Hotels.Where(x => x.City.ToLower() == city.ToLower());
        }

        public IEnumerable<Hotel> GetHotelsByRoomType(string roomType)
        {
            return AgodaDbContext.Hotels.Where(x => x.Room.ToLower() == roomType.ToLower());
        }

        public bool UpdateOrAddMapping(IEnumerable<Hotel> entities)
        {
            return true;
        }
    }
}