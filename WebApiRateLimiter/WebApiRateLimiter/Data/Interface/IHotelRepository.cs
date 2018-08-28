using System.Collections.Generic;
using WebApiRateLimiter.Data.Model;

namespace WebApiRateLimiter.Data.Interface
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<Hotel> GetAllHotels();

        IEnumerable<Hotel> GetHotelsByCity(string city);

        IEnumerable<Hotel> GetHotelsByRoomType(string roomType);
    }
}