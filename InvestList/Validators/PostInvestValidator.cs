using FluentValidation;
using InvestList.Models.V2;

namespace InvestList.Validators
{
    public class PostInvestValidator: AbstractValidator<PutInvestModel>
    {
        public PostInvestValidator()
        {
            RuleFor(vm => vm.AnnualInvestmentReturn)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповення")
                .GreaterThan(0)
                .WithMessage("Значення має бути позитивним");

            RuleFor(vm => vm.TotalInvestment)
                .GreaterThan(0)
                .When(x=>x.TotalInvestment.HasValue)
                .WithMessage("Значення має бути позитивним");

            RuleFor(vm => vm.MinInvestValues)
               .Must(HaveAtLeastOneNonNullAndPositiveValue)
               .WithMessage("Ви маєте вказати мінімальний поріг входу хоча б для якоїсь валюти. Значення має бути позитивним");

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
        }

        private static bool HaveAtLeastOneNonNullAndPositiveValue(IEnumerable<CurrencyView> acceptedCurrencies)
        {
            return acceptedCurrencies?.Any(value => value.MinValue != null && value.MinValue > 0) == true;
        }
    }
}
