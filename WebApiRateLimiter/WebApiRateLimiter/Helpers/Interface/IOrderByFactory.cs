using System.Collections.Generic;
using WebApiRateLimiter.Data.Model;

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