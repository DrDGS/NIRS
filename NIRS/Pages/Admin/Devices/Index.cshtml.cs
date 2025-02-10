using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Admin.Devices
{
    [Authorize(Roles = "Admin,Manager,Tech")]
    public class IndexModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public IndexModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IList<Device> Device { get;set; } = default!;
        
        //For full name searching
        public SelectList? Names { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }

        public SelectList? Types { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Type { get; set; }

        //For address searching
        public SelectList? Clubs { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Club { get; set; }

        //For phone number searching
        public SelectList? AreWorking { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? IsWorking { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> namesQuery = from d in _context.Device orderby d.Name select d.Name;
            IQueryable<string> typesQuery = from d in _context.Device orderby d.Type select d.Type;
            IQueryable<string> clubsQuery = from d in _context.Device orderby d.Club select d.Club.Address;
            IQueryable<bool> isWorkingQuery = from d in _context.Device orderby d.isWorking select d.isWorking;
            var devices = from d in _context.Device select d;

            if (User.IsInRole("Tech") || User.IsInRole("Manager"))
            {
                var clubClaim = User.Claims.FirstOrDefault(c => c.Type == "ClubId");
                if (clubClaim != null && int.TryParse(clubClaim.Value, out int clubId))
                {
                    ViewData["ClubId"] = clubId.ToString();
                    devices = devices.Where(d => d.ClubId.ToString() == ViewData["ClubId"]);
                }
            }
            if (!string.IsNullOrEmpty(Name))
            {
                devices = devices.Where(x => x.Name == Name);
            }
            if (!string.IsNullOrEmpty(Type))
            {
                devices = devices.Where(x => x.Type == Type);
            }
            if (Club != null)
            {
                devices = devices.Where(x => x.Club.Address == Club);
            }
            if (IsWorking != null)
            {
                devices = devices.Where(x => x.isWorking == IsWorking);
            }

            Names = new SelectList(await namesQuery.Distinct().ToListAsync());
            Types = new SelectList(await typesQuery.Distinct().ToListAsync());
            Clubs = new SelectList(await clubsQuery.Distinct().ToListAsync());
            AreWorking = new SelectList(await isWorkingQuery.Distinct().ToListAsync());

            Device = await devices
                .Include(d => d.Club).ToListAsync();
        }
    }
}
