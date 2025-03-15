using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace InvestList.Extensions;

public class GlobalCultureTemplatePageRouteModelConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        // Identify if this is the default page in the "Main" area.
        // Adjust the check as needed if your Index page is in a different location.
        var isDefaultPage = model.ViewEnginePath.Equals("/Index", System.StringComparison.OrdinalIgnoreCase)
                            && model.AreaName?.Equals("Main", System.StringComparison.OrdinalIgnoreCase) == true;

        if (isDefaultPage)
        {
            // Clear existing selectors and add a single selector for the default page.
            model.Selectors.Clear();
            model.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = "{culture:regex(^[a-z]{{2}}$)}"
                }
            });
            return;
        }

        // For all other pages, if any selector already contains "{culture", skip adding.
        if (model.Selectors.Any(s =>
                !string.IsNullOrEmpty(s.AttributeRouteModel.Template) &&
                s.AttributeRouteModel.Template.Contains("{culture")))
        {
            return;
        }

        // For pages with selectors that do not include culture, add a new selector with culture prefixed.
        int selectorCount = model.Selectors.Count;
        for (int i = 0; i < selectorCount; i++)
        {
            var selector = model.Selectors[i];
            var originalTemplate = selector.AttributeRouteModel.Template;
            string newTemplate;
            if (string.IsNullOrEmpty(originalTemplate))
            {
                newTemplate = "{culture:regex(^[a-z]{{2}}$)}";
            }
            else
            {
                newTemplate = AttributeRouteModel.CombineTemplates("{culture:regex(^[a-z]{{2}}$)}", originalTemplate);
            }

            model.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Order = -1, // Lower order so that explicit routes take precedence.
                    Template = newTemplate
                }
            });
        }
    }
}