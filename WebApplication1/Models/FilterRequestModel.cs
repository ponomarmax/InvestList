using Common;

namespace WebApplication1.Models
{
    public class FilterRequestModel
    {
        public Currency Currency{ get; set; }
        
        public decimal? MinInvestment { get; set; }
        
        public decimal? MaxInvestment { get; set; }
        
        public decimal? MinAnnualInvestmentReturn { get; set; }
        
        public decimal? MaxAnnualInvestmentReturn { get; set; }
        
        public int CurrentPage { get; set; }
    }
}
