using AutoMapper;
using WebApiRateLimiter.Data.Model;
using WebApiRateLimiter.Data.ViewModel;

namespace WebApiRateLimiter
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelViewModel>();
        }
    }
}