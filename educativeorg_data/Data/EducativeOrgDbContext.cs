using educativeorg_models.Models;
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

    }
}
