using AutoMapper;
using WebApiRateLimiter.Data.Model;
using WebApiRateLimiter.Data.ViewModel;

namespace WebApiRateLimiter
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // creating mapping with hotel model with hotel view model.
            CreateMap<Hotel, HotelViewModel>();
        }
    }
}