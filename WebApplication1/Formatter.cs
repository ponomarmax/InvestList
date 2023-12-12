namespace WebApplication1
{
    public static class InvestmentHelper
    {
        public static string FormatInvestmentDuration(int years, int months)
        {
            var result = "";

            if (years > 0)
            {
                result += FormatYears(years);
            }

            if (months > 0)
            {
                result += (result.Length > 0 ? " " : "") + FormatMonths(months);
            }

            return result;
        }

        private static string FormatYears(int years)
        {
            return years switch
            {
                1 => $"{years} рік",
                >= 2 and <= 4 => $"{years} роки",
                _ => $"{years} років"
            };
        }

        private static string FormatMonths(int months)
        {
            return months switch
            {
                1 => $"{months} місяць",
                >= 2 and <= 4 => $"{months} місяці",
                _ => $"{months} місяців"
            };
        }
    }
}
