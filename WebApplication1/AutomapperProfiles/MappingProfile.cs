using AutoMapper;
using DataAccess.Models;
using WebApplication1.Models;

namespace WebApplication1.AutomapperProfiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // CreateMap<TSource, TDestination>()
            CreateMap<InvestAd, GetAllAdsView>()
                .ForMember(x => x.InvestFields, y => y.MapFrom(z => z.History.Any() ?
                z.History.First().InvestFields.Select(x => x.InvestField.Title)
                : new string[] { }))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.CreatedAt.DateTime))
                .ForMember(x => x.Author, y => y.MapFrom(z => z.Author.Email));

            CreateMap<PostInvestAdViewModel, InvestAd>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.Author, y => y.Ignore())
                .ForMember(x => x.Published, y => y.MapFrom(z => true))
                .ForMember(x => x.History, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTimeOffset.UtcNow))
                .ForMember(x => x.UpdateAt, y => y.MapFrom(z => DateTimeOffset.UtcNow));

            CreateMap<PostInvestAdViewModel, InvestAdExtraInfo>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.InvestFields, y => y.Ignore())
                .ForMember(x => x.InvestAd, y => y.Ignore())
                .ForMember(x => x.InvestAdId, y => y.Ignore())
                .ForMember(x => x.AcceptedCurrencies,
                                y => y.Ignore()
                                //.MapFrom(z => z.AcceptedCurrencies.Select(u => new MinimalInvestEntrance
                                //{
                                //    Id = Guid.NewGuid(),
                                //    MinValue = u.Value,
                                //    Currency = u.Key
                                //}))
                                )
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTimeOffset.UtcNow));


            //PostInvestAdViewModel
            // Add additional mappings as needed
        }
    }
}
