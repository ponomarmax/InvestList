using System.Globalization;
using AutoMapper;
using Core.Entities;
using InvestList.Models;
using InvestList.Models.Comment;
using InvestList.Models.V2;
using InvestList.Services;
using Radar.Application.Models;
using Radar.Domain.Entities;
using CommentView = Radar.Application.Models.CommentView;
using GoogleAnalyticDataView = Radar.Application.Models.GoogleAnalyticDataView;
using LinkView = Radar.Application.Models.LinkView;
using PostLinkView = Radar.Application.Models.PostLinkView;
using PostView = Radar.Application.Models.PostView;
using PutPostTranslationModel = Radar.Application.Models.PutPostTranslationModel;
using TagView = Radar.Application.Models.TagView;

namespace InvestList.Mapper
{
    public class InvestProfile: Profile
    {
        public InvestProfile()
        {
            // DB->GET
            CreateMap<InvestPost, InvestView>()
                .ForMember(x => x.Post, s => s.MapFrom(x => x.Post));

            CreateMap<PostTags, TagView>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.TagId))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.Tag.Id));

            CreateMap<PostComment, CommentView>()
                .ForMember(x => x.AuthorId, s => s.MapFrom(x => x.UserId));

            CreateMap<PostCommentRequest, PostComment>()
                .ForMember(x => x.PostId, s => s.MapFrom(x => x.PostId))
                .ForMember(x => x.Text, s => s.MapFrom(x => x.Text))
                .ForMember(x => x.UserId, s => s.MapFrom(x => x.UserId));

            //DB->PUT
            CreateMap<MinInvestValue, CurrencyView>();
            CreateMap<CurrencyView, MinInvestValue>();
            CreateMap<InvestPost, PutInvestModel>()
                .ForMember(x => x.MinInvestValues, s => s.MapFrom(x => x.MinInvestValues));

            //PUT -> DB
            CreateMap<PutInvestModel, InvestPost>();
            CreateMap<PutInvestModel, Post>()
                ;
            
            CreateMap<PostFormModel, Post>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations))
                .ForMember(dest=>dest.Tags,opt=>opt.MapFrom(x=> x.SelectedTagIds.Select(y=>new PostTags(){ TagId = y })))
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.Links))
                .ForMember(dest => dest.PostType, opt => opt.Ignore())
                .ForMember(x => x.Tags,
                    s => s.MapFrom(x =>
                        x.SelectedTagIds == null ? null : x.SelectedTagIds.Select(t => new PostTags() { TagId = t })))
                .ForMember(x => x.Images,
                    s => s.MapFrom(x =>
                        x.ImageBase64 == null
                            ? null
                            : new List<ImageMetadata>
                            {
                                new ImageMetadata
                                {
                                    ImageObject = new ImageObject()
                                    {
                                        Image = Convert.FromBase64String(x.ImageBase64)
                                    }
                                }
                            }));
            CreateMap<PostTranslation, PutPostTranslationModel>();

            CreateMap<PutPostTranslationModel, PostTranslation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostId, opt => opt.Ignore());
            CreateMap<PostLinkModel, PostLink>()
                .ForMember(dest => dest.Follow, opt => opt.MapFrom(src => src.Follow ?? false));
            CreateMap<Post, PutPostModel>()
                .ForMember(x => x.ImageBase64,
                    s => s.MapFrom(x =>
                        x.Images.FirstOrDefault() == null
                            ? null
                            : Convert.ToBase64String(x.Images.FirstOrDefault().ImageObject.Image)))
                .ForMember(x => x.Title, s => s.MapFrom(x => x.Title))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.IsActive))
                .ForMember(x => x.Description, s => s.MapFrom(x => x.Description))
                .ForMember(x => x.TagIds, s => s.MapFrom(x => x.Tags));

            CreateMap<Post, PostFormModel>()
                .ForMember(x => x.ImageBase64,
                    s => s.MapFrom(x =>
                        x.Images.FirstOrDefault() == null
                            ? null
                            : Convert.ToBase64String(x.Images.FirstOrDefault().ImageObject.Image)))
                // .ForMember(x => x.Title, s => s.MapFrom(x => x.Title))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.IsActive))
                // .ForMember(x => x.Description, s => s.MapFrom(x => x.Description))
                .ForMember(x => x.SelectedTagIds, s => s.MapFrom(x => x.Tags.Select(t => t.TagId)));
            
            CreateMap<PutPostModel, Post>()
                .ForMember(x => x.Tags,
                    s => s.MapFrom(x =>
                        x.TagIds == null ? null : x.TagIds.Select(t => new PostTags() { TagId = Guid.Parse(t) })))
                .ForMember(x => x.Images,
                    s => s.MapFrom(x =>
                        x.ImageBase64 == null
                            ? null
                            : new List<ImageMetadata>
                            {
                                new ImageMetadata
                                {
                                    ImageObject = new ImageObject()
                                    {
                                        Image = Convert.FromBase64String(x.ImageBase64)
                                    }
                                }
                            }));


            // DB-> DB

            CreateMap<Post, Post>()
               .ForMember(x => x.Id, s => s.Ignore())
                .ForMember(x => x.PostType, s => s.Ignore())
                .ForMember(x => x.CreatedBy, s => s.Ignore())
                .ForMember(x => x.CreatedById, s => s.Ignore())
                .ForMember(x => x.CreatedAt, s => s.Ignore())
                .ForMember(x => x.UpdatedAt, s => s.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.Comments, s => s.Ignore())
                .ForMember(x => x.Slug, s => s.Ignore()); 

            CreateMap<InvestPost, InvestPost>()
                .ForMember(x => x.Id, s => s.Ignore())
                .ForMember(x => x.PostId, s => s.Ignore());
            CreateMap<User, UserView>();

          
            CreateMap<Post, PostView>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => GetTranslation(src).Title ?? string.Empty))
                .ForMember(dest => dest.TitleSeo,
                    opt => opt.MapFrom(src => GetTranslation(src).TitleSeo))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => GetTranslation(src).Description))
                .ForMember(dest => dest.DescriptionSeo,
                    opt => opt.MapFrom(src => GetTranslation(src).DescriptionSeo))                
                .ForMember(x=>x.GoogleAnalyticPostView, s => s.MapFrom(x=>x.GoogleAnalyticPostView))

                .ForMember(x => x.Image, s => s.MapFrom(x =>
                    x.Images.FirstOrDefault() == null
                        ? null
                        : ImageService.GetImageView2(x.Images.FirstOrDefault().Id, x)));

            CreateMap<PostLinkView, PostLink>();
            CreateMap<PostLink, LinkView>();
            CreateMap<PostLink, PostLinkView>();
            CreateMap<PostTags, TagView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src=>src.TagId))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => GetTranslation(src.Tag).Name ?? string.Empty));
            CreateMap<Tag, TagView>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => GetTranslation(src).Name ?? string.Empty));
            
            
            // Admin

            CreateMap<Post, AdminPostView>();
            CreateMap<PutAdminPost, Post>();
            
            //
            CreateMap<GoogleAnalyticPostView, GoogleAnalyticDataView>();
            CreateMap<GoogleAnalyticPostView, InvestList.Models.V2.GoogleAnalyticDataView>();
        }
        
        private static PostTranslation? GetTranslation(Post post)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return post.Translations?.FirstOrDefault(t => t.Language == culture)
                   ?? post.Translations?.FirstOrDefault(); // fallback
        }
        
        private static TagTranslation? GetTranslation(Tag post)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return post.Translations?.FirstOrDefault(t => t.Language == culture)
                   ?? post.Translations?.FirstOrDefault(); // fallback
        }
    }
    
}