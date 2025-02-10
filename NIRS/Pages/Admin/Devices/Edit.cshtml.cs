using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Admin.Devices
{
    [Authorize(Roles = "Admin,Manager,Tech")]
    public class EditModel : PageModel
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

        public EditModel(NIRS.Data.NIRSContext context)
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

            var device =  await _context.Device.FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }
            Device = device;
            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
            OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
                return Page();
            }

            _context.Attach(Device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(Device.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DeviceExists(int id)
        {
            return _context.Device.Any(e => e.Id == id);
        }
    }
}
