using AutoMapper;
using CVAHelper.Data.Interface;
using CVAHelper.Data.Model;
using CVAHelper.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [HttpGet("city")]
        public IEnumerable<HotelViewModel> GetHotelsByCity()
        {
            var hotels = _hotelRepository.GetAllHotels();
            return _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotels);
        }

        // GET api/room
        [HttpGet("room")]
        public IEnumerable<string> GetHotelsByRoomType()
        {
            return new string[] { "hotel3", "hotel4" };
        }
    }
}