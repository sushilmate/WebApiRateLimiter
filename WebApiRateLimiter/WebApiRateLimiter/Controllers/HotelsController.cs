using AutoMapper;
using CVAHelper.Data.Interface;
using CVAHelper.Data.Model;
using CVAHelper.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using WebApiRateLimiter.Attributes.Throttle;

namespace WebApiRateLimiter.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        private IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelsController(IMemoryCache memoryCache, IHotelRepository hotelRepository, IMapper mapper)
        {
            _memoryCache = memoryCache;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        // GET api/city
        [HttpGet("city/{cityName}")]
        [HttpGet("city/{cityName}/{orderByPriceAsc}")]
        [TypeFilter(typeof(ThrottleApiRateAttribute), Arguments = new object[] { "GetHotelsByCity" })]
        public IEnumerable<HotelViewModel> GetHotelsByCity(string cityName, string orderByPriceAsc = "")
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;

            List<Hotel> hotels = new List<Hotel>();

            if (orderByPriceAsc.ToLower() == "asc")
            {
                hotels = _hotelRepository.GetHotelsByCity(cityName).OrderBy(x => x.Price).ToList();
            }
            else if (orderByPriceAsc.ToLower() == "desc")
            {
                hotels = _hotelRepository.GetHotelsByCity(cityName).OrderByDescending(x => x.Price).ToList();
            }
            else
            {
                hotels = _hotelRepository.GetHotelsByCity(cityName).ToList();
            }

            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotels);
        }

        // GET api/room
        [HttpGet("room/{roomType}")]
        [HttpGet("room/{roomType}/{orderByPriceAsc}")]
        public IEnumerable<HotelViewModel> GetHotelsByRoomType(string roomType, string orderByPriceAsc = "")
        {
            if (string.IsNullOrWhiteSpace(roomType))
                return null;

            List<Hotel> hotels = new List<Hotel>();

            if (orderByPriceAsc.ToLower() == "asc")
            {
                hotels = _hotelRepository.GetHotelsByRoomType(roomType).OrderBy(x => x.Price).ToList();
            }
            else if (orderByPriceAsc.ToLower() == "desc")
            {
                hotels = _hotelRepository.GetHotelsByRoomType(roomType).OrderByDescending(x => x.Price).ToList();
            }
            else
            {
                hotels = _hotelRepository.GetHotelsByRoomType(roomType).ToList();
            }

            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotels);
        }
    }
}