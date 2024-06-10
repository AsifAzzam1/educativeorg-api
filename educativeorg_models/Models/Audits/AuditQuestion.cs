using educativeorg_models.Helper;
using educativeorg_models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Models.Audits
{
    public class AuditQuestion:BaseEntity
    {
        public Guid SectionId { get; set; }
        public AuditSection AuditSection { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public string QuestionType { get { return _type; } set { _type = value.VerifyValueFrom<AuditQuestionTypeEnum>(); } }
        private string _type = "";

        public ICollection<AuditAnswer> Answers { get; set; }


    }
}
