using Common;

namespace WebApplication1.Models
{
    public class FilterNewsRequestModel
    {
        public List<string> TagIds { get; set; }
        public int CurrentPage { get; set; } = 1;
    }
}
