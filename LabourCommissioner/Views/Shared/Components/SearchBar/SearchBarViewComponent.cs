using Microsoft.AspNetCore.Mvc;

namespace LabourCommissioner.Views.Shared.Components.SearchBar
{
    public class SearchBarViewComponent : ViewComponent
    {
        public SearchBarViewComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync(SPager SearchPager, bool BottomBar = false)
        {
            if (BottomBar == true)
                return View("BottomBar", SearchPager);
            else
                return View("Default", SearchPager);
        }
    }

}
