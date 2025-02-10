using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        [BindProperty(SupportsGet = true)]
        public string? SearchFullNameString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchEmailAddressString { get; set; }

        public async Task OnGetAsync()
        {
            var users = from u in _context.User select u;

            if (!SearchFullNameString.IsNullOrEmpty())
            {
                users = users.Where(u => u.FullName.Contains(SearchFullNameString));
            }
            if (!SearchEmailAddressString.IsNullOrEmpty())
            {
                users = users.Where(u => u.EmailAddress.Contains(SearchEmailAddressString));
            }

            _User = await users.ToListAsync();
        }
    }
}
