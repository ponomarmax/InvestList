using Common;
using FluentValidation;
using InvestList.Models.Comment;

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
        }
    }
}
