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

namespace NIRS.Pages.Admin.Users
{
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public class IndexModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public IndexModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IList<User> _User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var user = _context.User;

            if (User.IsInRole("Manager") || User.IsInRole("Cashier"))
            {
                
            }

            _User = await _context.User
                .Include(u => u.Club).ToListAsync();
        }
    }
}
