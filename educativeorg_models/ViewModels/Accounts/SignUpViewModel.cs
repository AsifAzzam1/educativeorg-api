﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels.Accounts
{
    public class SignUpViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid RoleId { get; set; }
        public CompanyViewModel CompanyInfo { get; set; }
    }

    public class CompanyViewModel 
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class SignInViewModel 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResponseViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string Expiry { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }

    }

}
