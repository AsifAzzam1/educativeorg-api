using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Audits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.Audit.AuditServices
{
    public interface IAuditService
    {
        Task<ResponseViewModel<AuditViewModel>> AddAudit(AuditViewModel input);
        Task<ResponseViewModel<object>> ChangeActiveStatus(Guid Id);
        Task<ResponseViewModel<AuditViewModel>> GetById(Guid Id);
        Task<ResponseViewModel<AuditViewModel>> UpdateAudit(AuditViewModel input);
    }
}
