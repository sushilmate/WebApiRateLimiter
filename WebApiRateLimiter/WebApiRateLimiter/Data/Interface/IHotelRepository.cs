using CVAHelper.Data.Model;
using System.Collections.Generic;

namespace CVAHelper.Data.Interface
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<Hotel> GetAllHotels();

        bool UpdateOrAddMapping(IEnumerable<Hotel> gidGsrMappings);
    }
}