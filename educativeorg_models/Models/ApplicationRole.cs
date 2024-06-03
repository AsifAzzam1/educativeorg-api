using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models
{
    public class ApplicationRole:IdentityRole<Guid>
    {
        public bool Active { get; set; }

        public List<Permissions> Permissions { get; set; }
        public List<ApplicationUser> Users { get; set; }


    }
}
