using InvestList.Models.V2;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace InvestList
{
    public static class SeoHelper
    {
        private const int titleForIndex = 3;
        private const int titleForDescription = 5;
        private const int maxTitleSize = 65;
        private const int maxDescriptionSize = 160;

        public static void SetupPostViewSeoDetails(this ViewDataDictionary viewData, PostView entity)
        {
            var title = entity.TitleSeo ?? entity.Title;
            var desc = Regex.Replace(entity.DescriptionSeo ?? entity.Description, "<.*?>", string.Empty);
            
            viewData["CustomTitle"] = maxTitleSize < title.Length
                ? title.Substring(0, maxTitleSize)
                : title;
            viewData["CustomDescription"] = maxDescriptionSize < desc?.Length
                ? desc?.Substring(0, maxDescriptionSize)
                : desc;
        }
        
        public static void SetupListPostViewSeoDetails(this ViewDataDictionary ViewData, IEnumerable<PostView> entities)
        {
            var finalTitle = "Інвестиційні оголошення";

            if (entities != null && entities.Any())
            {
                finalTitle = string.Join(' ', entities.Take(titleForIndex).Select(x => x.Title));
                if (maxTitleSize < finalTitle.Length)
                    finalTitle = finalTitle.Substring(0, maxTitleSize);
            }

            ViewData["CustomTitle"] = finalTitle;
            
            var finalDesc = "Бізнес шукає інвесторів в багатьох оголошеннях";
            if (entities != null && entities.Any())
            {
                finalDesc = string.Join(' ',
                    entities.Skip(titleForIndex).Take(titleForDescription).Select(x => x.Title));
                if (maxDescriptionSize < finalDesc.Length)
                    finalDesc = finalDesc.Substring(0, maxDescriptionSize);
            }

            ViewData["CustomDescription"] = finalDesc;
        }
    }
}