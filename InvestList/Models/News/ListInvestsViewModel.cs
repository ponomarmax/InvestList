using InvestList.Models.News;

namespace InvestList.Models
{
    public class ListNewsViewModel
    {
        public IEnumerable<GetNewsViewModel> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
