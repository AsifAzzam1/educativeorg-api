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
        Task<ResponseViewModel<PaginateResponseModel<GetUserViewModel>>> GetAll(FilterViewModel filter);
        Task<ResponseViewModel<GetUserViewModel>> GetById(Guid Id);
        Task<ResponseViewModel<GetUserViewModel>> ToggleStatus(Guid userId);
        Task<ResponseViewModel<GetUserViewModel>> Update(GetUserViewModel user);
    }
}
