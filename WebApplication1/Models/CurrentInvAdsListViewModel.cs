namespace WebApplication1.Models
{
    public class CurrentInvAdsListViewModel
    {
        public IEnumerable<GetAllAdsView> Entities { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
