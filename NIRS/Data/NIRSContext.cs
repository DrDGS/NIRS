using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NIRS.Models;

namespace NIRS.Data
{
    public class NIRSContext : DbContext
    {
        public NIRSContext (DbContextOptions<NIRSContext> options)
            : base(options)
        {
        }

        public DbSet<NIRS.Models.User> User { get; set; } = default!;
        public DbSet<NIRS.Models.Club> Club { get; set; } = default!;
        public DbSet<NIRS.Models.Device> Device { get; set; } = default!;
        public DbSet<NIRS.Models.Review> Review { get; set; } = default!;
        public DbSet<NIRS.Models.Rate> Rate { get; set; } = default!;
        public DbSet<NIRS.Models.Order> Order { get; set; } = default!;
    }
}
