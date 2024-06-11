using AutoMapper;
using educativeorg_models.Models;
using educativeorg_models.Models.Audits;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_models.ViewModels.Audits;
using System.Reflection.Metadata.Ecma335;

namespace educativeorg_api
{
    public class AutoMapperConfiguration:Profile
    {
        public AutoMapperConfiguration() 
        {
            CreateMap<ApplicationUser, GetUserViewModel>()
                .ForMember(dest=> dest.UserRole,opt => opt.MapFrom(src => MapUserRoles(src)));
            CreateMap<ApplicationRole, RoleOutputViewModel>();

            CreateMap<Company,CompanyViewModel>().ReverseMap();

            CreateMap<AuditModel, AuditViewModel>().ReverseMap();
        }

        private List<KeyValueEntity<Guid>>? MapUserRoles(ApplicationUser user) 
        {
            if (user.Roles == null || user.Roles.Count == 0)
                return null;

            return user.Roles.Select(_ => new KeyValueEntity<Guid>
            {
                Id = _.Id,
                Value = _.Name,
                Active = true,
            }).ToList();
        }
    }
}
