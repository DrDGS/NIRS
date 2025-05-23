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

namespace NIRS.Pages.Admin.Users
{
    [Authorize(Roles = "Admin,Manager,Cashier")]
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
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
