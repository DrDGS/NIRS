using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;

namespace NIRS.Pages.Admin.Orders
{
    [Authorize(Roles = "Admin,Manager,Cashier")]
    public class IndexModel : PageModel
    {
        private readonly NIRS.Data.NIRSContext _context;

        public IndexModel(NIRS.Data.NIRSContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchClubAddressString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchClientEmailAdressesString { get; set; }

        public SelectList? RateNames { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? RateName { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> rateNamesQuery = from o in _context.Order orderby o.Rate select o.Rate.Name;
            var orders = from o in _context.Order select o;

            if (User.IsInRole("Cashier") || User.IsInRole("Manager"))
            {
                var clubClaim = User.Claims.FirstOrDefault(c => c.Type == "ClubId");
                if (clubClaim != null && int.TryParse(clubClaim.Value, out int clubId))
                {
                    ViewData["ClubId"] = clubId.ToString();
                    orders = orders.Where(o => o.ClubId.ToString() == ViewData["ClubId"]);
                }
            }
            if (!string.IsNullOrEmpty(SearchClubAddressString))
            {
                orders = orders.Where(x => x.Club.Address.Contains(SearchClubAddressString));
            }
            if (!string.IsNullOrEmpty(SearchClientEmailAdressesString))
            {
                orders = orders.Where(o => o.User.EmailAddress.Contains(SearchClientEmailAdressesString));
            }
            if (!string.IsNullOrEmpty(RateName))
            {
                orders = orders.Where(x => x.Rate.Name == RateName); //нифига не работает
            }
            if (CreationDate != null)
            {
                orders = orders.Where(x => x.CreationDate.Value.Date == CreationDate.Value.Date); //не работает?
            }

            RateNames = new SelectList(await rateNamesQuery.Distinct().ToListAsync());
            Order = await orders
                .Include(o => o.User)
                .Include(o => o.Club)
                .Include(o => o.Rate).ToListAsync();
        }
    }
}
