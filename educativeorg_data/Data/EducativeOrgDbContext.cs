using educativeorg_models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_data.Data
{
    public class EducativeOrgDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public EducativeOrgDbContext(DbContextOptions<EducativeOrgDbContext> opts):base(opts)
        {
            
        }


        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationRole>().HasMany(x => x.Permissions).WithMany().UsingEntity<RolePermissions>();
            builder.Entity<ApplicationRole>().HasMany(x => x.Users).WithMany().UsingEntity<IdentityUserRole<Guid>>();
            builder.Entity<ApplicationUser>().HasMany(x => x.Roles).WithMany().UsingEntity<IdentityUserRole<Guid>>();

            builder.Entity<Permissions>().HasMany(x => x.Roles).WithMany().UsingEntity<RolePermissions>();
        }

    }
}
