using Common;

namespace InvestList.Models
{
    public class FilterNewsRequestModel
    {
        public List<string> TagIds { get; set; }
        public int CurrentPage { get; set; } = 1;
    }
}
