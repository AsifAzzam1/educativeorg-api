using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models
{
    public sealed class RolePermissions
    {
        public int Id { get; set; }
        public Guid RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public int PermissionId { get; set; }
        public Permissions Permission { get; set; }
    }
}
