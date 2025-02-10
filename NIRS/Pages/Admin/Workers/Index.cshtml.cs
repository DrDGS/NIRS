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

namespace NIRS.Pages.Admin.Workers
{
    [Authorize(Roles = "Admin,Manager")]
    public class IndexModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public IndexModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IList<Worker> Worker { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchFullNameString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchEmailAddressString { get; set; }

        public SelectList? Clubs { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Club { get; set; }

        public SelectList? Roles { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Role { get; set; }

        public async Task OnGetAsync()
        {
            var workers = from w in _context.Worker select w;
            IQueryable<string> rolesQuery = from u in _context.User orderby u.Role select u.Role;
            IQueryable<string> clubsQuery = from d in _context.Device orderby d.Club select d.Club.Address;

            if (User.IsInRole("Manager"))
            {
                var clubClaim = User.Claims.FirstOrDefault(c => c.Type == "ClubId");
                if (clubClaim != null && int.TryParse(clubClaim.Value, out int clubId))
                {
                    ViewData["ClubId"] = clubId.ToString();
                    workers = workers.Where(w => w.ClubId.ToString() == ViewData["ClubId"]);
                }
            }

            if (!SearchFullNameString.IsNullOrEmpty())
            {
                workers = workers.Where(w => w.FullName.Contains(SearchFullNameString));
            }
            if (!SearchEmailAddressString.IsNullOrEmpty())
            {
                workers = workers.Where(w => w.EmailAddress.Contains(SearchEmailAddressString));
            }
            if (Club != null)
            {
                workers = workers.Where(w => w.Club.Address == Club);
            }
            if (!Role.IsNullOrEmpty())
            {
                workers = workers.Where(w => w.Role == Role);
            }

            Roles = new SelectList(await rolesQuery.Distinct().ToListAsync());
            Clubs = new SelectList(await clubsQuery.Distinct().ToListAsync());
            Worker = await _context.Worker
                .Include(w => w.Club).ToListAsync();
        }
    }
}
