using Radar.Application.Models;

namespace InvestList.Models.V2;

public class InvestPostWithSimilarResponseDto
{
    public InvestPostDetailDto? InvestPost { get; set; }
    public Dictionary<string, List<PostDetailDto>> SimilarPosts { get; set; }
}