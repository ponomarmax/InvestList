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
               .Must(BeAValidBase64String).WithMessage("Завантажте зображення");
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
