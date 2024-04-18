namespace WebApplication1.Models.Invest
{
    public class ListInvestsViewModel
    {
        public IEnumerable<GetAllAdsView> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public FilterRequestModel FilterModel { get; set; }
    }
}
