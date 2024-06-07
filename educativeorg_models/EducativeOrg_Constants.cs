

namespace educativeorg_models
{
    public static class EducativeOrg_Constants
    {
        public static string JWtkey = "this is a very long long key, you can chnage it whenever you want";



        public static Dictionary<PermissionModules, List<PermissionsSet>> Permissions = new Dictionary<PermissionModules, List<PermissionsSet>>
        {
            {
                PermissionModules.Account,
                new List<PermissionsSet>
                {
                   PermissionsSet.AddUsers, PermissionsSet.DeleteUsers, PermissionsSet.UpdateUsers, PermissionsSet.GetUserById, educativeorg_models.PermissionsSet.GetAllUsers
                }
            },
            {
                PermissionModules.Permissions,
                new List<PermissionsSet>
                {
                   PermissionsSet.GetPermissiosForRole,PermissionsSet.SetPermissionsForRole 
                }
            }


        };

   

    }
    
}
