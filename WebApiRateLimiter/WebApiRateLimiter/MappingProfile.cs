using AutoMapper;
using CVAHelper.Data.Model;
using CVAHelper.Data.ViewModel;

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