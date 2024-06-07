using AutoMapper;
using educativeorg_models.Models;
using educativeorg_models.ViewModels.Accounts;

namespace educativeorg_api
{
    public class AutoMapperConfiguration:Profile
    {
        public AutoMapperConfiguration() 
        {
            CreateMap<ApplicationUser, GetUserViewModel>();
            CreateMap<ApplicationRole, RoleOutputViewModel>();
        }
    }
}
