using FluentValidation;
using InvestList.Models.V2;

namespace InvestList.Validators
{
    public class PostValidator: AbstractValidator<PutPostModel>
    {
        public PostValidator()
        {
            RuleFor(vm => vm.Title)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповення")
                .Length(10, 300)
                .WithMessage("Кількість символів має бути в діпазоні від 10 до 60");

            RuleFor(vm => vm.TagIds)
               .NotNull()
               .WithMessage("Вкажіть хоча б один тег, що характерезує Ваш бізнес");

            RuleFor(x => x.ImageBase64)
               .Must(ImageValidator.BeAValidBase64String)
               .When(x=>!string.IsNullOrWhiteSpace(x.ImageBase64)).WithMessage("Завантажте зображення");
        }
    }
}
