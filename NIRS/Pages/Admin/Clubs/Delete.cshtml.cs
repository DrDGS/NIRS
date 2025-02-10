using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Clubs
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public DeleteModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Club Club { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club.FirstOrDefaultAsync(m => m.Id == id);

            if (club is not null)
            {
                Club = club;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club.FindAsync(id);
            if (club != null)
            {
                Club = club;
                _context.Club.Remove(Club);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
