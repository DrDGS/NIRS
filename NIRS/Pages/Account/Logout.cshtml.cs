using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NIRS.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Sign out the user by clearing the authentication cookie
            await HttpContext.SignOutAsync();

            // Redirect to the home page or any other desired page
            return RedirectToPage("/Index");
        }
    }
}
