using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;

namespace NIRS.Pages.Admin.Dashboard
{
    [Authorize(Roles = "Admin,Manager,Cashier,Tech")]
    public class IndexModel : PageModel
    {
        private readonly NIRSContext _context;

        public IndexModel(NIRSContext context) => _context = context;

        public int? TotalOrders { get; private set; }
        public int? TotalRevenue { get; private set; }
        public string? TopUserName { get; private set; }
        public int? MostTimePlayed { get; private set; }
        public string MostOrderedRateName { get; private set; } 
        public int TotalOrdersForMostOrderedRate { get; private set; } 

        public async Task OnGetAsync()
        {
            var orders = _context.Order;
            var users = _context.User;

            TotalOrders = await orders.CountAsync();

            TotalRevenue = await orders
            .Where(order => order.Rate != null && order.Minutes > 0) // Ensure valid data
            .Select(order => order.Minutes * (order.Rate.RublesPerMinute ?? 0)) // Calculate revenue per order
            .SumAsync();

            var topUser = await users
                .Where(user => user.Orders.Any(order => order.Minutes > 0)) // Ensure user has valid orders
                .Select(user => new
                {
                    User = user,
                    TotalMinutes = user.Orders.Sum(order => order.Minutes) // Calculate total minutes for each user
                })
                .OrderByDescending(x => x.TotalMinutes) // Sort by total minutes in descending order
                .FirstOrDefaultAsync(); // Get the user with the most minutes

            if (topUser != null)
            {
                TopUserName = topUser.User.FullName;
                MostTimePlayed = topUser.TotalMinutes;
            }

            var mostOrderedRate = await orders
                .Where(order => order.Rate != null) // Ensure Rate is not null
                .GroupBy(order => order.Rate) // Group orders by Rate
                .Select(group => new
                {
                    Rate = group.Key,
                    OrderCount = group.Count() // Count the number of orders for each Rate
                })
                .OrderByDescending(x => x.OrderCount) // Sort by order count in descending order
                .FirstOrDefaultAsync(); // Get the most ordered Rate

            if (mostOrderedRate != null && mostOrderedRate.Rate != null)
            {
                MostOrderedRateName = mostOrderedRate.Rate.Name; // Set the name of the most ordered Rate
                TotalOrdersForMostOrderedRate = mostOrderedRate.OrderCount; // Set the total orders for that Rate
            }
        }
    }
}
