using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages
{
    [Authorize]
    public class MakeOrderModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        private readonly ILogger _logger;

        public MakeOrderModel(NIRS.Data.NIRSContext context, ILogger<MakeOrderModel> logger)
        {
            _context = context;
            _logger = logger;
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
                ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
                ViewData["RateId"] = new SelectList(_context.Rate, "Id", "Description");
                return Page();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                ModelState.AddModelError("", "User ID not found.");
                ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
                ViewData["RateId"] = new SelectList(_context.Rate, "Id", "Description");
                return Page();
            }

            // Fetch the user from the database
            var currentUser = await _context.User.FindAsync(userId);

            if (currentUser == null)
            {
                return RedirectToPage("/Account/AddBalance");
            }

            IQueryable<Device> devices;
            Rate _rate = await _context.Rate.FindAsync(Order.RateId);
            devices = _context.Device.Where(d => d.ClubId == Order.ClubId && d.Type == _rate.DeviceType && d.isWorking);
            if (!await devices.AnyAsync())
            {
                ModelState.AddModelError("", "Нет подходящих устройств.");
                ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
                ViewData["RateId"] = new SelectList(_context.Rate, "Id", "Description");
                return Page();
            }

            int? balanceRequired = Order.Minutes * _rate.RublesPerMinute;
            if (currentUser.Balance < balanceRequired)
            {
                ModelState.AddModelError("", "Недостаточно средств.");
                ViewData["ClubId"] = new SelectList(_context.Club, "Id", "Address");
                ViewData["RateId"] = new SelectList(_context.Rate, "Id", "Description");
                return Page();
            }
            currentUser.Balance -= balanceRequired;

            Order.UserId = currentUser.Id;

            _context.Order.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
