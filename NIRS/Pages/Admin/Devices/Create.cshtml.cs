﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Admin.Devices
{
    [Authorize(Roles = "Admin,Manager,Tech")]
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
            ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
            OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
            return Page();
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OptionsList = Options.Select(option => new SelectListItem { Value = option, Text = option });
                return Page();
            }

            _context.Device.Add(Device);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
