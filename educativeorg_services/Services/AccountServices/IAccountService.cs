using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.AccountServices
{
    public interface IAccountService
    {
        Task<ResponseViewModel<LoginResponseViewModel>> SignIn(SignInViewModel input);
        Task<ResponseViewModel<GetUserViewModel>> SignUp(SignUpViewModel input);
    }
}
