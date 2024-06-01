using Common;
using Core;
using FluentValidation;
using InvestList.Models.Invest;
using SixLabors.ImageSharp;

namespace InvestList.Validators
{
    public class PostInvestAdViewModelValidator: AbstractValidator<PostInvestAdViewModel>
    {
        public PostInvestAdViewModelValidator()
        {
            RuleFor(vm => vm.Title)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповення")
                .Length(10, 60)
                .WithMessage("Кількість символів має бути в діпазоні від 10 до 60");

            RuleFor(vm => vm.AnnualInvestmentReturn)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповення")
                .GreaterThan(0)
                .WithMessage("Значення має бути позитивним");

            RuleFor(vm => vm.TotalInvestment)
                .GreaterThan(0)
                .When(x=>x.TotalInvestment.HasValue)
                .WithMessage("Значення має бути позитивним");

            RuleFor(vm => vm.AcceptedCurrencies)
               .Must(HaveAtLeastOneNonNullAndPositiveValue)
               .WithMessage("Ви маєте вказати мінімальний поріг входу хоча б для якоїсь валюти. Значення має бути позитивним");

            RuleFor(vm => vm.Tags)
               .NotNull()
               .WithMessage("Вкажіть хоча б один тег, що характерезує Ваш бізнес");

            RuleFor(x => x.InvestDurationYears)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => x.HasValue && x > 0)
                .When(x => x.InvestDurationMonths == null || x.InvestDurationMonths <= 0)
                .WithMessage("Вкажіть скільки років триває інвестаційний період. Значення має бути позитивним");

            RuleFor(x => x.InvestDurationMonths)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => x.HasValue && x > 0)
                .When(x => x.InvestDurationYears == null || x.InvestDurationYears <= 0)
                .WithMessage("Вкажіть скільки місяців триває інвестаційний період. Значення має бути позитивним");

            RuleFor(x => new { x.InvestDurationYears, x.InvestDurationMonths })
               .Cascade(CascadeMode.StopOnFirstFailure)
               .Must(pair => pair.InvestDurationMonths >= 0 && pair.InvestDurationYears >= 0)
               .When(pair => pair.InvestDurationYears.HasValue && pair.InvestDurationMonths.HasValue)
               .WithMessage("Значення має бути позитивним");

            RuleFor(x => x.ImageBase64)
               .Must(ImageValidator.BeAValidBase64String)
               .When(x=>!string.IsNullOrWhiteSpace(x.ImageBase64)).WithMessage("Завантажте зображення");
        }

        private static bool HaveAtLeastOneNonNullAndPositiveValue(IDictionary<Currency, decimal?> acceptedCurrencies)
        {
            return acceptedCurrencies?.Values.Any(value => value != null && value > 0) == true;
        }
    }
}
