using Microsoft.AspNetCore.Mvc;
using NIRS.Data;
using NIRS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace NIRS.ViewComponents
{
    public class UserBalanceViewComponent : ViewComponent
    {
        private readonly NIRSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserBalanceViewComponent(NIRSContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Content("Not logged in");
            }

            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return Content("User not found");
            }

            // Pass the balance to the View Component's view
            return View(user.Balance);
        }
    }
}