using CVAHelper.Data.DatabaseContext;
using CVAHelper.Data.Interface;
using CVAHelper.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace CVAHelper.Data.Repository
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

        public IEnumerable<Hotel> GetAllHotelsByCity(string city)
        {
            return AgodaDbContext.Hotels.Where(x => x.City.ToLower() == city.ToLower());
        }

        public IEnumerable<Hotel> GetAllHotelsByRoomType(string roomType)
        {
            return AgodaDbContext.Hotels.Where(x => x.Room.ToLower() == roomType.ToLower());
        }

        public bool UpdateOrAddMapping(IEnumerable<Hotel> entities)
        {
            return true;
        }
    }
}