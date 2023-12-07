using Common;
using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators
{
    public class PostInvestAdViewModelValidator: AbstractValidator<PostInvestAdViewModel>
    {
        public PostInvestAdViewModelValidator()
        {
            RuleFor(vm => vm.Title)
                .NotEmpty()
                .WithMessage("Поле обов'язкове для заповення")
                .Length(10, 200)
                .WithMessage("Кількість символів має бути в діпазоні (10,200)");

            RuleFor(vm => vm.TotalInvestment)
                .NotNull()
                .WithMessage("Поле обов'язкове для заповення")
                .GreaterThan(0)
                .WithMessage("Значення має бути позитивним");

            RuleFor(vm => vm.AcceptedCurrencies)
               .Must(HaveAtLeastOneNonNullAndPositiveValue)
               .WithMessage("Ви маєте вказати мінімальний поріг входу хоча б для якоїсь валюти. Значення має бути позитивним");

            RuleFor(vm => vm.InvestFields)
               .NotNull()
               .WithMessage("Вкажіть хоча б одну галузь інвестування");

            RuleFor(x => x.InvestDurationYears)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => x.HasValue && x > 0)
                .When(x => x.InvestDurationMonths == null || x.InvestDurationMonths <= 0)
                .WithMessage("Either InvestDurationYears or InvestDurationMonths must have a positive value.");

            RuleFor(x => x.InvestDurationMonths)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => x.HasValue && x > 0)
                .When(x => x.InvestDurationYears == null || x.InvestDurationYears <= 0)
                .WithMessage("Either InvestDurationYears or InvestDurationMonths must have a positive value.");
        }
       
        private static bool HaveAtLeastOneNonNullAndPositiveValue(IDictionary<Currency, decimal?> acceptedCurrencies)
        {
            return acceptedCurrencies?.Values.Any(value => value != null && value > 0) == true;
        }

        private static bool HaveAtLeastOneNonNullAndPositiveValue(IDictionary<InvestPeriodSpan, int?> acceptedCurrencies)
        {
            return acceptedCurrencies?.Values.Any(value => value != null && value > 0) == true;
        }
    }
}
