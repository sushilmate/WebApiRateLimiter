using AutoMapper;
using CVAHelper.Data.Interface;
using CVAHelper.Data.Model;
using CVAHelper.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebApiRateLimiter.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        private IHotelRepository _hotelRepository;

        // GET api/city
        [HttpGet("city/{cityName}")]
        [HttpGet("city/{cityName}/{orderByPriceAsc}")]
        public IEnumerable<HotelViewModel> GetHotelsByCity(string cityName, string orderByPriceAsc = "")
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;

            List<Hotel> hotels = new List<Hotel>();

            if (orderByPriceAsc.ToLower() == "asc")
            {
                hotels = _hotelRepository.GetAllHotelsByCity(cityName).OrderBy(x => x.Price).ToList();
            }
            else if (orderByPriceAsc.ToLower() == "desc")
            {
                hotels = _hotelRepository.GetAllHotelsByCity(cityName).OrderByDescending(x => x.Price).ToList();
            }
            else
            {
                hotels = _hotelRepository.GetAllHotelsByCity(cityName).ToList();
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
                hotels = _hotelRepository.GetAllHotelsByRoomType(roomType).OrderBy(x => x.Price).ToList();
            }
            else if (orderByPriceAsc.ToLower() == "desc")
            {
                hotels = _hotelRepository.GetAllHotelsByRoomType(roomType).OrderByDescending(x => x.Price).ToList();
            }
            else
            {
                hotels = _hotelRepository.GetAllHotelsByRoomType(roomType).ToList();
            }

            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotels);
        }
    }
}