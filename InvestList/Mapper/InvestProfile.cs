using AutoMapper;
using Core.Entities;
using InvestList.Models.V2;
using Radar.Application.Models;
using Radar.Domain.Entities;
using CommentView = Radar.Application.Models.CommentView;

namespace InvestList.Mapper
{
    public class InvestProfile: Profile
    {
        public InvestProfile()
        {
            // DB->GET
            CreateMap<InvestPost, InvestPostDetailDto>()
                .ForMember(x => x.Post, s => s.MapFrom(x => x.Post));

            CreateMap<PostComment, CommentView>()
                .ForMember(x => x.AuthorId, s => s.MapFrom(x => x.UserId));

            CreateMap<PostCommentRequest, PostComment>()
                .ForMember(x => x.PostId, s => s.MapFrom(x => x.PostId))
                .ForMember(x => x.Text, s => s.MapFrom(x => x.Text))
                .ForMember(x => x.UserId, s => s.MapFrom(x => x.UserId));

            //DB->PUT
            CreateMap<MinInvestValue, CurrencyView>();
            CreateMap<CurrencyView, MinInvestValue>();
            CreateMap<InvestPost, InvestPostDto>()
                .ForMember(x => x.MinInvestValues, s => s.MapFrom(x => x.MinInvestValues));

            //PUT -> DB
            CreateMap<InvestPostDto, InvestPost>();
            CreateMap<InvestPostDto, Post>()
                ;
            
            CreateMap<InvestPost, InvestPost>()
                .ForMember(x => x.Id, s => s.Ignore())
                .ForMember(x => x.PostId, s => s.Ignore());
            CreateMap<User, UserView>();
            
            // Admin

            CreateMap<Post, AdminPostView>();
            CreateMap<PutAdminPost, Post>();
            
            //
        }
       
    }
    
}
