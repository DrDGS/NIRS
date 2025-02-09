using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Admin.Devices
{
    public class DetailsModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public DetailsModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

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
    }
}
