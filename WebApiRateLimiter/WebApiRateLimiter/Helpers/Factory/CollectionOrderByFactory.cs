using CVAHelper.Data.Model;
using System.Collections.Generic;
using System.Linq;
using WebApiRateLimiter.Helpers.Interface;

namespace WebApiRateLimiter.Helpers.Factory
{
    public class CollectionOrderByFactory : IOrderByFactory
    {
        IOrderBy IOrderByFactory.Create(string orderByPriceAsc)
        {
            switch (orderByPriceAsc)
            {
                case "desc":
                    return new OrderByDescending();

                default:
                    return new OrderByAscending();
            }
        }
    }

    public class OrderByAscending : IOrderBy
    {
        public IEnumerable<Hotel> Order(IEnumerable<Hotel> hotel)
        {
            return hotel.OrderBy(x => x.Price);
        }
    }

    public class OrderByDescending : IOrderBy
    {
        public IEnumerable<Hotel> Order(IEnumerable<Hotel> hotel)
        {
            return hotel.OrderByDescending(x => x.Price);
        }
    }
}