using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApiRateLimiter.Attributes.Throttle;
using WebApiRateLimiter.Data.Interface;
using WebApiRateLimiter.Data.Model;
using WebApiRateLimiter.Data.ViewModel;
using WebApiRateLimiter.Helpers.Interface;

namespace WebApiRateLimiter.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly IOrderByFactory _orderByFactory;

        public HotelsController(IHotelRepository hotelRepository, IMapper mapper, IOrderByFactory orderByFactory)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _orderByFactory = orderByFactory;
        }

        [HttpGet("city/{cityName}")]
        [TypeFilter(typeof(ThrottleApiRateAttribute), Arguments = new object[] { "GetHotelsByCity" })]
        public IEnumerable<HotelViewModel> GetHotelsByCity(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;

            var hotels = _hotelRepository.GetHotelsByCity(cityName);

            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotels);
        }

        // GET api/city
        // can have multiple routes here but swagger showing optional parameter as mandatory field so created new api
        //[HttpGet("city/{cityName}")]
        [HttpGet("city/{cityName}/{orderByPriceAsc}")]
        [TypeFilter(typeof(ThrottleApiRateAttribute), Arguments = new object[] { "GetHotelsByCity" })]
        public IEnumerable<HotelViewModel> GetHotelsByCity(string cityName, string orderByPriceAsc)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;

            List<Hotel> hotels = _hotelRepository.GetHotelsByCity(cityName).ToList();

            return GetHotelViewModels(orderByPriceAsc, hotels);
        }

        [HttpGet("room/{roomType}")]
        [TypeFilter(typeof(ThrottleApiRateAttribute), Arguments = new object[] { "GetHotelsByRoomType" })]
        public IEnumerable<HotelViewModel> GetHotelsByRoomType(string roomType)
        {
            if (string.IsNullOrWhiteSpace(roomType))
                return null;

            List<Hotel> hotels = _hotelRepository.GetHotelsByRoomType(roomType).ToList();

            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotels);
        }

        // GET api/room
        //[HttpGet("room/{roomType}")]
        [HttpGet("room/{roomType}/{orderByPriceAsc}")]
        [TypeFilter(typeof(ThrottleApiRateAttribute), Arguments = new object[] { "GetHotelsByRoomType" })]
        public IEnumerable<HotelViewModel> GetHotelsByRoomType(string roomType, string orderByPriceAsc)
        {
            if (string.IsNullOrWhiteSpace(roomType))
                return null;

            List<Hotel> hotels = _hotelRepository.GetHotelsByRoomType(roomType).ToList();

            return GetHotelViewModels(orderByPriceAsc, hotels);
        }

        private IEnumerable<HotelViewModel> GetHotelViewModels(string orderByPriceAsc, List<Hotel> hotels)
        {
            var orderBy = _orderByFactory.Create(orderByPriceAsc);
            // mapper maps the model to viewmodel or viewmodel to model.
            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(orderBy.Order(hotels));
        }
    }
}