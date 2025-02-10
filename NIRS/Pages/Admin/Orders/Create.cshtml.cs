using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Admin.Orders
{
    [Authorize(Roles = "Admin,Manager,Cashier,Client")]
    public class CreateModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public CreateModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
            ViewData["RateId"] = new SelectList(_context.Rate, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Order.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
