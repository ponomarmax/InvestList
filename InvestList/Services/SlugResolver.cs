using Core.Entities;
using Radar.Application;
using Radar.Domain.Interfaces;

namespace InvestList.Services;

public class SlugResolver : ISlugResolver
{
    public string GetPrefixSlug(string postType)
    {
        if (Enum.TryParse<PostType>(postType, out var parsed))
        {
            return parsed switch
            {
                PostType.News => "news",
                PostType.InvestAd => "invest",
                PostType.Blacklist => "blacklist",
                _ => throw new NotImplementedException($"Prefix not defined for {parsed}")
            };
        }

        throw new ArgumentException($"Invalid postType: {postType}", nameof(postType));
    }
    
    public string? GetPostTypeFromPrefixSlug(string prefix)
    {
        return prefix.ToLowerInvariant() switch
        {
            "news" => PostType.News.ToString(),
            "invest" => PostType.InvestAd.ToString(),
            "blacklist" => PostType.Blacklist.ToString(),
            _ => null
        };
    }
}