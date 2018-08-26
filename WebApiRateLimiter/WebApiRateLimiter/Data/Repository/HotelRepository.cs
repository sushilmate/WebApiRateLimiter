using CVAHelper.Data.DatabaseContext;
using CVAHelper.Data.Interface;
using CVAHelper.Data.Model;
using System.Collections.Generic;

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

        public bool UpdateOrAddMapping(IEnumerable<Hotel> entities)
        {
            return true;
        }
    }
}