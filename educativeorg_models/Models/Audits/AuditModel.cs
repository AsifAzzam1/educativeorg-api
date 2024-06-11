using educativeorg_models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models.Audits
{
    public class AuditModel : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public string Title { get; set; }
        public ICollection<AuditSection> Sections { get; set; }

    }
}
