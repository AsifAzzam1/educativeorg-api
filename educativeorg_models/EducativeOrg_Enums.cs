using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models
{
    public enum PermissionModules
    {
        Account,
        Permissions
    }
    public enum PermissionsSet
    {
        //Accounts
        AddUsers = 1,
        DeleteUsers,
        UpdateUsers,
        GetUserById,
        GetAllUsers,
        //Permissions
        GetPermissiosForRole,
        SetPermissionsForRole
        


    }
}
