using AutoMapper;
using DataAccess.Models;
using InvestList.Models;

namespace InvestList.AutomapperProfiles
{
    public class TagProfile: Profile
    {
        public TagProfile()
        {
            CreateMap<CustomHeader, TagView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TagId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tag.Name));
            CreateMap<Tag, TagView>();
        }
    }
}