using InvestList.Models.Invest;

namespace InvestList.Models.V2
{
    public class ListInvestView
    {
        public IEnumerable<InvestView> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public FilterRequestModel FilterRequestModel { get; set; }
    }
}