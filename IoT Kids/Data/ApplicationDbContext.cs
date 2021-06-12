using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoT_Kids.Models.Memberships;
using IoT_Kids.Models.Users;
using IoT_Kids.Models.Payments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IoT_Kids.Models.LMS;

namespace IoT_Kids.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<MembershipPlan> MembershipPlan { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<UserMembershipLog> UserMembershipLog { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Lesson> Lesson { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
