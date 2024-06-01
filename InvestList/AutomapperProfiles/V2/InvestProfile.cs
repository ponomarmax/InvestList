using AutoMapper;
using Core.Entities;
using InvestList.Models;
using InvestList.Models.Comment;
using InvestList.Models.News;
using InvestList.Models.V2;
using InvestList.Services;

namespace InvestList.AutomapperProfiles.V2
{
    public class InvestProfile: Profile
    {
        public InvestProfile()
        {
            // DB->GET
            CreateMap<InvestPost, InvestView>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.Post.Id))
                .ForMember(x => x.PostType, s => s.MapFrom(x => x.Post.PostType))
                .ForMember(x => x.CreatedById, s => s.MapFrom(x => x.Post.CreatedById))
                // .ForMember(x => x.ImageBase64,
                //     s => s.MapFrom(x =>
                //         x.Post.Images.FirstOrDefault() == null ? null : x.Post.Images.FirstOrDefault().ImageBase64))
                .ForMember(x => x.Image, s => s.MapFrom(x =>
                    x.Post.ImagesV2.FirstOrDefault() == null
                        ? null
                        : ImageService.GetImageView(x.Post.ImagesV2.FirstOrDefault().Id, x.Post)))
                .ForMember(x => x.CreatedAt, s => s.MapFrom(x => x.Post.CreatedAt))
                .ForMember(x => x.UpdateAt, s => s.MapFrom(x => x.Post.UpdatedAt))
                .ForMember(x => x.Slug, s => s.MapFrom(x => x.Post.Slug))
                .ForMember(x => x.Title, s => s.MapFrom(x => x.Post.Title))
                .ForMember(x => x.Description, s => s.MapFrom(x => x.Post.Description))
                .ForMember(x => x.Tags, s => s.MapFrom(x => x.Post.Tags))
                .ForMember(x => x.Comments, s => s.MapFrom(x => x.Post.Comments));

            CreateMap<PostTags, TagView>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.TagId))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.Tag.Name));

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
                .ForMember(x => x.ImageBase64,
                    s => s.MapFrom(x =>
                        x.Post.ImagesV2.FirstOrDefault() == null
                            ? null
                            : Convert.ToBase64String(x.Post.ImagesV2.FirstOrDefault().ImageObject.Image)))
                .ForMember(x => x.Title, s => s.MapFrom(x => x.Post.Title))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.Post.IsActive))
                .ForMember(x => x.MinInvestValues, s => s.MapFrom(x => x.MinInvestValues))
                .ForMember(x => x.Description, s => s.MapFrom(x => x.Post.Description))
                .ForMember(x => x.TagIds, s => s.MapFrom(x => x.Post.Tags));

            //PUT -> DB
            CreateMap<PutInvestModel, InvestPost>();
            CreateMap<PutInvestModel, Post>()
                .ForMember(x => x.Tags,
                    s => s.MapFrom(x =>
                        x.TagIds == null ? null : x.TagIds.Select(t => new PostTags() { TagId = Guid.Parse(t) })))
                .ForMember(x => x.ImagesV2,
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
                            }))
                ;
            CreateMap<Post, PutPostModel>()
                .ForMember(x => x.ImageBase64,
                    s => s.MapFrom(x =>
                        x.ImagesV2.FirstOrDefault() == null
                            ? null
                            : Convert.ToBase64String(x.ImagesV2.FirstOrDefault().ImageObject.Image)))
                .ForMember(x => x.Title, s => s.MapFrom(x => x.Title))
                .ForMember(x => x.IsActive, s => s.MapFrom(x => x.IsActive))
                .ForMember(x => x.Description, s => s.MapFrom(x => x.Description))
                .ForMember(x => x.TagIds, s => s.MapFrom(x => x.Tags));

            CreateMap<PutPostModel, Post>()
                .ForMember(x => x.Tags,
                    s => s.MapFrom(x =>
                        x.TagIds == null ? null : x.TagIds.Select(t => new PostTags() { TagId = Guid.Parse(t) })))
                .ForMember(x => x.ImagesV2,
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


            CreateMap<NewsToTags, TagView>()
                .ForMember(x => x.Id, s => s.MapFrom(x => x.TagId))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.Tag.Name));
            CreateMap<Post, PostView>()
                // .ForMember(x => x.ImageBase64,
                //     s => s.MapFrom(x =>
                //         x.Images.FirstOrDefault() == null ? null : x.Images.FirstOrDefault().ImageBase64))
                .ForMember(x => x.Image, s => s.MapFrom(x =>
                    x.ImagesV2.FirstOrDefault() == null
                        ? null
                        : ImageService.GetImageView(x.ImagesV2.FirstOrDefault().Id, x)));

            CreateMap<News, PostView>()
                .ForMember(x => x.CreatedAt, s => s.MapFrom(x => x.CreatedAt.DateTime));

            CreateMap<PostLinkView, PostLink>();
            CreateMap<PostLink, LinkView>();
            CreateMap<PostLink, PostLinkView>();
        }
    }
}