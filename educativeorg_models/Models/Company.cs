using educativeorg_models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models
{
    public class Company:BaseEntity
    {
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

    }
}
