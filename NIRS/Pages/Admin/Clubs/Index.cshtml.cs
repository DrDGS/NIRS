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

namespace NIRS.Pages.Clubs
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public IndexModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IList<Club> Club { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchAddressString { get; set; }

        public async Task OnGetAsync()
        {
            var clubs = from c in _context.Club select c;

            if (!string.IsNullOrEmpty(SearchAddressString))
            {
                clubs = clubs.Where(x => x.Address.Contains(SearchAddressString));
            }

            Club = await clubs.ToListAsync();
        }
    }
}
