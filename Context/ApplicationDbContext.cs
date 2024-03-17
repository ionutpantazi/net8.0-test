using Microsoft.EntityFrameworkCore;
using mvc.Models;
using System.Collections.Generic;

namespace mvc.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions) { }
        public DbSet<EventModel> EventModel { get; set; }
    }
}
