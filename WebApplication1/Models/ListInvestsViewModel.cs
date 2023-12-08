namespace WebApplication1.Models
{
    public class ListInvestsViewModel
    {
        public IEnumerable<GetAllAdsView> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
