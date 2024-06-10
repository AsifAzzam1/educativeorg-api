using educativeorg_models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models.Audits
{
    public class AuditSection:BaseEntity
    {
        public Guid AuditId { get; set; }
        public Audit Audit { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public ICollection<AuditQuestion> Questions { get; set; }
    }
}
