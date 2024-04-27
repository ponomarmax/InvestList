namespace InvestList.Models
{
    public class ListSearchResultViewModel
    {
        public IEnumerable<SearchResultViewModel> Entities { get; set; }

        public string SearchTerm { get; set; }

        public PaginationInfo PaginationInfo { get; set; }
    }
}
