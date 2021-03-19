using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PPSPS.Areas.Identity.Data;
using PPSPS.Models;

namespace PPSPS.Data
{
    public class AuthDBContext : IdentityDbContext<PPSPSUser>
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options)
            : base(options)
        {
        }

        public DbSet<PPSPSUser> Users { get; set; }
        public DbSet<PPSPSTask> Tasks { get; set; }
        public DbSet<PPSPSAssignment> Assignments { get; set; }
        public DbSet<PPSPSClass> Classes { get; set; }
        public DbSet<PPSPSSubject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
