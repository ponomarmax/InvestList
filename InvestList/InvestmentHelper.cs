using System.Security.Claims;
using System.Text.Json;
using Core;
using Core.Entities;
using InvestList.Models;
using Microsoft.AspNetCore.Identity;
using Radar.Domain.Entities;

namespace InvestList
{
    public static class InvestmentHelper
    {

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
        
        public static UserRequestInfo GetRequestInfo(HttpContext httpContext, string userId, UserDetectionInfo detectionInfo)
        {
            var headers = new Dictionary<string, string>();
            foreach (var header in httpContext.Request.Headers)
            {
                headers[header.Key] = header.Value;
            }

            var cookies = new Dictionary<string, string>();
            foreach (var cookie in httpContext.Request.Cookies)
            {
                cookies[cookie.Key] = cookie.Value;
            }

            return new UserRequestInfo
            {
                UserId = userId,
                IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpContext.Request.Headers["User-Agent"],
                Headers = JsonSerializer.Serialize(headers),
                Cookies = JsonSerializer.Serialize(cookies),
                MouseMoved = detectionInfo.MouseMoved,
                NavigatorWebdriver = detectionInfo.NavigatorWebdriver,
                ScreenHeight = detectionInfo.ScreenHeight,
                ScreenWidth = detectionInfo.ScreenWidth,
                HasChrome = detectionInfo.HasChrome,
                TimeSpent = detectionInfo.TimeSpent,
            };
        }
    }
}