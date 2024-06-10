using educativeorg_models.ViewModels.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<ApplicationRole> Roles { get; set; }
    }
}
