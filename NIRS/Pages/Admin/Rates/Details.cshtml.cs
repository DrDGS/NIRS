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

namespace NIRS.Pages.Admin.Rates
{
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public class DetailsModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public DetailsModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public Rate Rate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rate = await _context.Rate.FirstOrDefaultAsync(m => m.Id == id);

            if (rate is not null)
            {
                Rate = rate;

                return Page();
            }

            return NotFound();
        }
    }
}
