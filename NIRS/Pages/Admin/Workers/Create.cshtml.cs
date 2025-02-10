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

namespace NIRS.Pages.Admin.Workers
{
    [Authorize(Roles = "Admin,Manager")]
    public class CreateModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;
        public IEnumerable<SelectListItem> OptionsList { get; set; }

        List<string> Options = new()
            {
                "Admin",
                "Manager",
                "Cashier",
                "Tech",
                "User"
            };

        public CreateModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
            OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
            return Page();
        }

        [BindProperty]
        public Worker Worker { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
                return Page();
            }

            _context.Worker.Add(Worker);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
