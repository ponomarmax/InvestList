using System.Security.Claims;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace InvestList
{
    public static class InvestmentHelper
    {
        public static Dictionary<string, string> GetPaginationData(int pageIndex = 1, IEnumerable<Guid> tagIds = null)
        {
            var param = new Dictionary
                <string, string>();
            if (pageIndex > 1)
                param.Add("pageIndex", $"{pageIndex}");
            if ((tagIds != null && tagIds.Any()))
                param.Add("tagIds", string.Join(",", tagIds));
            return param;
        }

        public static async Task<bool> CanEditInvestPost(this UserManager<User> userManager, User user, InvestPost db)
        {
            if (user == null) return false;
            return await userManager.CanEditPost(user) || db.Post.CreatedById == user.Id;
        }
        
        public static async Task<bool> CanEditPost(this UserManager<User> userManager, User user)
        {
            if (user == null) return false;
            return await userManager.IsInRoleAsync(user, Const.AdminRole);
        }

        public static async Task<bool> CanEditInvestPost(this UserManager<User> userManager,
            ClaimsPrincipal? user,
            InvestPost db)
        {
            if (user == null) return false;
            var userDb = await userManager.GetUserAsync(user);
            return await userManager.CanEditInvestPost(userDb, db);
        }
        
        public static async Task<bool> CanEditPost(this UserManager<User> userManager,
            ClaimsPrincipal? user)
        {
            if (user == null) return false;
            var userDb = await userManager.GetUserAsync(user);
            return await userManager.CanEditPost(userDb);
        }

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

        public static string DisplayCurrencySymbol(Currency currency)
        {
            if (currency == Currency.UAH)
            {
                return "₴";
            }
            else if (currency == Currency.USD)
            {
                return "$";
            }

            return null;
            // Add more conditions for other currencies if needed
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