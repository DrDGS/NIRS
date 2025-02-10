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

namespace NIRS.Pages.Admin.Devices
{
    [Authorize(Roles = "Admin,Manager,Tech")]
    public class DeleteModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public DeleteModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device.FirstOrDefaultAsync(m => m.Id == id);

            if (device is not null)
            {
                Device = device;

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

            var device = await _context.Device.FindAsync(id);
            if (device != null)
            {
                Device = device;
                _context.Device.Remove(Device);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
