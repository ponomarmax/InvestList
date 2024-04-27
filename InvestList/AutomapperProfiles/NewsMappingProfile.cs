using AutoMapper;
using DataAccess.Models;
using InvestList.Models;
using InvestList.Models.News;

namespace InvestList.AutomapperProfiles
{
    public class NewsMappingProfile: Profile
    {
        public NewsMappingProfile()
        {
            CreateMap<PostNewsViewModel, News>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTimeOffset.UtcNow))
                .ForMember(x => x.Tags, y => y.MapFrom(z => z.Tags.Select(x => new NewsToTags() { TagId = Guid.Parse(x) })))
                ;

            CreateMap<News, PostNewsViewModel>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ImageBase64, y => y.MapFrom(src => src.ImageBase64))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.Tags, y => y.MapFrom(z => z.Tags.Select(x => x.TagId.ToString())));

            CreateMap<News, GetNewsViewModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Email))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.CreatedAt.DateTime))
                .ForMember(x => x.Tags,
                    y => y.MapFrom(z => z.Tags.Select(x => new TagView() { Id = x.TagId, Name = x.Tag.Name })));
        }
    }
}