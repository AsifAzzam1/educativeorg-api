using AutoMapper;
using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models.Helper;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.CompanyServices
{
    public class CompanyService:ICompanyService
    {
        private readonly EducativeOrgDbContext _context;
        private readonly IMapper _Mapper;
        private readonly IHttpContextAccessor _httpcontextaccessor;

        public CompanyService(EducativeOrgDbContext context, IMapper mapper, IHttpContextAccessor httpcontextaccessor)
        {
            _context = context;
            _Mapper = mapper;
            _httpcontextaccessor = httpcontextaccessor;
        }

        public async Task<ResponseViewModel<CompanyViewModel>> AddCompany(CompanyViewModel input) 
        {
            try
            {
                Company company = _Mapper.Map<Company>(input);
                company.InitiazlizeBaseColumns(/*_httpcontextaccessor.GetUserId()!.Value*/);

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                return new ResponseViewModel<CompanyViewModel> 
                {
                    Message = "Company Created",
                    Data = _Mapper.Map<CompanyViewModel>(company)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
