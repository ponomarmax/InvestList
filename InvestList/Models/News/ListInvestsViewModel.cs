using WebApplication1.Models.News;

namespace WebApplication1.Models
{
    public class ListNewsViewModel
    {
        public IEnumerable<GetNewsViewModel> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
