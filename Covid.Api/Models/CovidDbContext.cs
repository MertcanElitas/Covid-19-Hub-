using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid.Api.Models
{
    public class CovidDbContext : DbContext
    {
        public CovidDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Covid> Covids { get; set; }
    }
}
