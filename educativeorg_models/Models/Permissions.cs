using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models
{
    public sealed class Permissions
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ApplicationRole> Roles { get; set; }

    }
}
