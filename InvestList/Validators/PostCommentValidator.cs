using Common;
using FluentValidation;
using InvestList.Models.Comment;
using InvestList.Models.News;
using SixLabors.ImageSharp;

namespace InvestList.Validators
{
    public class PostCommentRequestValidator: AbstractValidator<PostCommentRequest>
    {
        public PostCommentRequestValidator()
        {
            RuleFor(vm => vm.Text)
                .NotNull()
                .WithMessage("Поле обов'язкове для заповення")
                .Length(10000)
                .WithMessage("Кількість символів має бути не більше 10000");

            RuleFor(request => request.InvestAdId)
                .Null().When(request => request.NewsId.HasValue)
                .WithMessage("InvestAdId should not be present when NewsId is provided.");

            RuleFor(request => request.NewsId)
                .Null().When(request => request.InvestAdId.HasValue)
                .WithMessage("NewsId should not be present when InvestAdId is provided.");
        }
    }
}
