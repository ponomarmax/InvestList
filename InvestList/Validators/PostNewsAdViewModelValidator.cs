using Common;
using FluentValidation;
using InvestList.Models.News;
using SixLabors.ImageSharp;

namespace InvestList.Validators
{
    public class PostNewsAdViewModelValidator: AbstractValidator<PostNewsViewModel>
    {
        public PostNewsAdViewModelValidator()
        {
            RuleFor(vm => vm.Title)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповення")
                .Length(1, 200)
                .WithMessage("Кількість символів має бути не більше 200");

            RuleFor(vm => vm.Description)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповнення");

            RuleFor(x => x.ImageBase64)
               .Must(ImageValidator.BeAValidBase64String).WithMessage("Завантажте зображення");
        }
    }
}
