using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.UserServices
{
    public interface IUserService
    {
        Task<ResponseViewModel<GetUserViewModel>> GetById(Guid Id);
        Task<ResponseViewModel<object>> ToggleStatus(Guid userId);
        Task<ResponseViewModel<GetUserViewModel>> Update(GetUserViewModel user);
    }
}
