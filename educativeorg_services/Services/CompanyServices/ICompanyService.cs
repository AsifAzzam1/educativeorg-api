using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.CompanyServices
{
    public interface ICompanyService
    {
        Task<ResponseViewModel<CompanyViewModel>> AddCompany(CompanyViewModel input);
    }
}
