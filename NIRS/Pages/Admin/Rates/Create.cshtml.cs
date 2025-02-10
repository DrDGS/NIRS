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

namespace NIRS.Pages.Admin.Rates
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public IEnumerable<SelectListItem> OptionsList { get; set; }

        List<string> Options = new()
            {
                "PC",
                "PS4",
                "PS5",
                "XBOX",
            };

        public CreateModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
            return Page();
        }

        [BindProperty]
        public Rate Rate { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
                return Page();
            }

            _context.Rate.Add(Rate);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
