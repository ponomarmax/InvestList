using AutoMapper;
using DataAccess.Models;
using System.Linq.Expressions;
using WebApplication1.Models;

namespace WebApplication1.AutomapperProfiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<InvestAd, GetAllAdsView>()
                .ForMember(x => x.InvestFields, y=>y.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestFields).Select(x=>x.InvestField.Title)))
                .ForMember(x => x.InvestDurationYears, y=>y.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestDurationYears)))
                .ForMember(x => x.InvestDurationMonths, y=>y.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestDurationMonths)))
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
                .ForMember(x => x.InvestDurationMonths, y => y.MapFrom(z => z.InvestDurationMonths.HasValue ? z.InvestDurationMonths - z.InvestDurationMonths / 12 * 12 : 0))
                .ForMember(x => x.InvestDurationYears, y => y.MapFrom(z => z.InvestDurationYears.HasValue ? z.InvestDurationYears + z.InvestDurationMonths / 12 : 0))
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.InvestAd, y => y.Ignore())
                .ForMember(x => x.InvestAdId, y => y.Ignore())
                .ForMember(x => x.AcceptedCurrencies, y => y
                                    .MapFrom(z => z.AcceptedCurrencies
                                        .Where(pair => pair.Value.HasValue)
                                        .Select(pair => new MinimalInvestEntrance
                                        {
                                            Id = Guid.NewGuid(),
                                            Currency = pair.Key,
                                            MinValue = pair.Value.Value
                                        })
                                        .ToList()))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTimeOffset.UtcNow))
                .ForMember(x => x.InvestFields, y => y.MapFrom(z => z.InvestFields.Select(x => new InvestAdExtraInfoInvestField
                {
                    InvestFieldId = Guid.Parse(x)
                })));

            CreateMap<InvestAd, InvestAdViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.DateTime))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => src.UpdateAt.DateTime))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.Title)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.Description)))
                .ForMember(dest => dest.SpendInvestDesc, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.SpendInvestDesc)))
                .ForMember(dest => dest.ProfitPaymentScheme, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.ProfitPaymentScheme)))
                .ForMember(dest => dest.OtherInfo, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.OtherInfo)))
                .ForMember(dest => dest.AcceptedCurrencies, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.AcceptedCurrencies)))
                .ForMember(dest => dest.TotalInvestment, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.TotalInvestment)))
                .ForMember(dest => dest.InvestFields, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestFields.Select(y => y.InvestField))));

            CreateMap<InvestField, InvestFieldView>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<InvestAd, PostInvestAdViewModel>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.Title)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.Description)))
                .ForMember(dest => dest.SpendInvestDesc, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.SpendInvestDesc)))
                .ForMember(dest => dest.ProfitPaymentScheme, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.ProfitPaymentScheme)))
                .ForMember(dest => dest.OtherInfo, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.OtherInfo)))
                .ForMember(dest => dest.AcceptedCurrencies, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.AcceptedCurrencies.ToDictionary(k => k.Currency, v => v.MinValue))))
                .ForMember(dest => dest.TotalInvestment, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.TotalInvestment)))
                .ForMember(dest => dest.InvestDurationYears, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestDurationYears)))
                .ForMember(dest => dest.InvestDurationMonths, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestDurationMonths)))
                .ForMember(dest => dest.InvestFields, opt => opt.MapFrom(src => GetLastHistoryItemProperty(src, x => x.InvestFields.Select(y => y.InvestField.Id))));

            CreateMap<InvestField, InvestFieldView>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

        }

        private static TProperty? GetLastHistoryItemProperty<T, TProperty>(
               T source, Expression<Func<InvestAdExtraInfo, TProperty>> propertySelector)
               where T : InvestAd
        {
            var lastItem = source.History.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            return lastItem != null ? propertySelector.Compile()(lastItem) : default;
        }
    }
}
