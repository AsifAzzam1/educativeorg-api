using AutoMapper;
using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models.Helper;
using educativeorg_models.Models.Audits;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Audits;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.Audit.AuditServices
{
    public class AuditService:IAuditService
    {
        private EducativeOrgDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(EducativeOrgDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseViewModel<AuditViewModel>> AddAudit(AuditViewModel input) 
        {
            try
            {
                var audit = _mapper.Map<AuditModel>(input);
                audit.CompanyId = _httpContextAccessor.GetCompanyId()!.Value;
                audit.InitiazlizeBaseColumns(_httpContextAccessor.GetUserId()!.Value);

                _context.Audits.Add(audit);
                await _context.SaveChangesAsync();

                return new ResponseViewModel<AuditViewModel> 
                {
                    Message = "Audit Created",
                    Data = _mapper.Map<AuditViewModel>(audit)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseViewModel<AuditViewModel>> UpdateAudit(AuditViewModel input)
        {
            try
            {
                var auditToupdate = await _context.Audits.FirstAsync("Audit not found", _ => _.Id == input.Id);

                auditToupdate = _mapper.Map(input,auditToupdate);
                auditToupdate.InitiazlizeBaseColumns(_httpContextAccessor.GetUserId()!.Value);

                _context.Audits.Update(auditToupdate);
                await _context.SaveChangesAsync();

                return new ResponseViewModel<AuditViewModel>
                {
                    Message = "Audit Created",
                    Data = _mapper.Map<AuditViewModel>(auditToupdate)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseViewModel<AuditViewModel>> GetById(Guid Id) 
        {
            try
            {
                var audit = await _context.AuditAnswers.FirstAsync("audit not found", _ => _.Id == Id);
                return new ResponseViewModel<AuditViewModel> 
                {
                    Message = "audit found",
                    Data = _mapper.Map<AuditViewModel>(audit)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel<object>> ChangeActiveStatus(Guid Id)
        {
            try
            {
                var audit = await _context.AuditAnswers.FirstAsync("audit not found", _ => _.Id == Id);
                audit.ToggleEntity<AuditModel>(_context, _httpContextAccessor.GetUserId()!.Value);
                return new ResponseViewModel<object>
                {
                    Message = "audit found",
                    Data = _mapper.Map<AuditViewModel>(audit)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
