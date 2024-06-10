using educativeorg_models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models.Audits
{
    public sealed class AuditAnswer:BaseEntity
    {

        public Guid QuestionId { get; set; }
        public AuditQuestion Question { get; set; }
        public string Title { get; set; }
        public int Sequance { get; set; }
    }
}
