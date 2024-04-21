using AutoMapper;
using DataAccess.Models;
using WebApplication1.Models.News;

namespace WebApplication1.AutomapperProfiles
{
    public class NewsMappingProfile: Profile
    {
        public NewsMappingProfile()
        {
            CreateMap<PostNewsViewModel, News>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTimeOffset.UtcNow))
                .ForMember(x => x.Tags, y => y.MapFrom(z =>
                    z.Tags.Select(x =>
                        new NewsToTags() { TagId = Guid.Parse(x) })))
                ;

            CreateMap<News, GetNewsViewModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Email))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.CreatedAt.DateTime))
                .ForMember(x => x.Tags,
                    y => y.MapFrom(z => z.Tags.Select(x => new TagView() { Id = x.TagId, Name = x.Tag.Name })));
        }
    }
}