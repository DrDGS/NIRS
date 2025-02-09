using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NIRS.Pages.Admin.Dashboard
{
    [Authorize(Roles = "Admin,Manager,Cashier,Tech")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
