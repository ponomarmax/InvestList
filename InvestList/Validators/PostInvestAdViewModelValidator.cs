using Common;
using FluentValidation;
using SixLabors.ImageSharp;
using WebApplication1.Models.Invest;

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

            RuleFor(vm => vm.InvestFields)
               .NotNull()
               .WithMessage("Вкажіть хоча б одну галузь інвестування");

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
               .Must(BeAValidBase64String)
               .When(x=>!string.IsNullOrWhiteSpace(x.ImageBase64)).WithMessage("Завантажте зображення");
        }

        private static bool HaveAtLeastOneNonNullAndPositiveValue(IDictionary<Currency, decimal?> acceptedCurrencies)
        {
            return acceptedCurrencies?.Values.Any(value => value != null && value > 0) == true;
        }

        private bool BeAValidBase64String(string base64String)
        {
            // Check if the string is a valid base64 string
            if (string.IsNullOrWhiteSpace(base64String))
                return false;

            // A simple regex check for a valid base64 string
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9+/]*={0,2}$");
            if (!regex.IsMatch(base64String))
                return false;

            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (Image image = Image.Load(ms))
                    {
                        // Additional image validation or processing can be performed here
                    }
                }

                return true;
            }
            catch (Exception)
            {
                // An exception occurred, indicating that the base64 string is not a valid image.
                return false;
            }
        }
    }
}
