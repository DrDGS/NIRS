using Microsoft.AspNetCore.Mvc.RazorPages;
using NIRS.Data;
using NIRS.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;

namespace NIRS.Pages.Account
{
    [Authorize(Roles = "User")]
    public class AddBalanceModel : PageModel
    {
        private readonly NIRSContext _context;

        public AddBalanceModel(NIRSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int AmountToAdd { get; set; } // The amount to add to the balance

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the current user's ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                ModelState.AddModelError("", "User ID not found.");
                return Page();
            }

            // Fetch the user from the database
            var currentUser = await _context.User.FindAsync(userId);

            if (currentUser == null)
            {
                ModelState.AddModelError("", "User not found.");
                return Page();
            }

            // Update the user's balance
            currentUser.Balance += AmountToAdd;
            ViewData["Balance"] = currentUser.Balance;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect to a success page or reload the current page
            return RedirectToPage("/Index"); // Replace with your desired page
        }
    }
}