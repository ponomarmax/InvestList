using Common;

namespace InvestList.Models.V2
{
    public class PutPostModel
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public IEnumerable<string>? TagIds { get; set; }

        public string? ImageBase64 { get; set; }

        public bool IsActive { get; set; }
    }
}
