using WebApiRateLimiter.Data.Model;
using System.Collections.Generic;

namespace WebApiRateLimiter.Data.Interface
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<Hotel> GetAllHotels();

        bool UpdateOrAddMapping(IEnumerable<Hotel> gidGsrMappings);

        IEnumerable<Hotel> GetHotelsByCity(string city);

        IEnumerable<Hotel> GetHotelsByRoomType(string roomType);
    }
}