using Core;
using InvestList.Models.V2;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Invest;

public class BaseInvestUpsertPage(ITagService tagService, ISanitizerService sanitizerService):  BaseUpsertPage(tagService, sanitizerService)
{
    [BindProperty]
    public PutInvestModel InvestPost { get; set; } = new();
    
    protected void Prepare()
    {
        // Populate MinInvestValues with all currencies
        var supportedCurrencies = Enum.GetValues(typeof(Currency)).Cast<Currency>();
        foreach (var currency in supportedCurrencies)
        {
            if (InvestPost.MinInvestValues.FirstOrDefault(x => x.Currency == currency) == null)
                InvestPost.MinInvestValues.Add(new CurrencyView
                {
                    Currency = currency,
                    MinValue = null
                });
        }
    }
}