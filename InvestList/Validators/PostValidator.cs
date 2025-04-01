using FluentValidation;
using InvestList.Models.V2;
using Radar.Application.Models;

namespace InvestList.Validators
{
    public class PostValidator: AbstractValidator<PostFormModel>
    {
        public PostValidator()
        {
            RuleFor(vm => vm.SelectedTagIds)
               .NotNull()
               .WithMessage("Вкажіть хоча б один тег, що характерезує Ваш бізнес");

            RuleFor(x => x.ImageBase64)
               .Must(ImageValidator.BeAValidBase64String)
               .When(x=>!string.IsNullOrWhiteSpace(x.ImageBase64)).WithMessage("Завантажте зображення");
            
            RuleFor(x => x.Translations)
                .Must(translations =>
                    translations != null &&
                    translations.Any(t =>
                        !string.IsNullOrWhiteSpace(t.Title) &&
                        !string.IsNullOrWhiteSpace(t.Description)))
                .WithMessage("Має бути хоча б один переклад з заповненими Title і Description");

            RuleForEach(x => x.Translations).ChildRules(translation =>
            {
                translation.RuleFor(t => t.Title)
                    .NotEmpty()
                    .When(t => !string.IsNullOrWhiteSpace(t.Description))
                    .WithMessage("Якщо заповнено Description, потрібно заповнити і Title");

                translation.RuleFor(t => t.Description)
                    .NotEmpty()
                    .When(t => !string.IsNullOrWhiteSpace(t.Title))
                    .WithMessage("Якщо заповнено Title, потрібно заповнити і Description");
            });
        }
    }
}
