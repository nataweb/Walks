using AutoMapper;

namespace Walks.API.Profiles
{
    public class RegionsProfile: Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Region,Models.DTO.Region>()
              .ForMember(dest => dest.Id,options => options.MapFrom(src => src.Id));
        }
    }
}
