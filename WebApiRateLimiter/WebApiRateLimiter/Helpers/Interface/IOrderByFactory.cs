using WebApiRateLimiter.Data.Model;
using System.Collections.Generic;

namespace WebApiRateLimiter.Helpers.Interface
{
    public interface IOrderByFactory
    {
        IOrderBy Create(string orderByPriceAsc);
    }

    public interface IOrderBy
    {
        IEnumerable<Hotel> Order(IEnumerable<Hotel> hotel);
    }
}