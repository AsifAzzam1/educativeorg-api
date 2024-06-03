

namespace educativeorg_models
{
    public static class EducativeOrg_Constants
    {
        public static string JWtkey = "this is a very long long key, you can chnage it whenever you want";



        public static Dictionary<PermissionModules, List<PermissionsSet>> Permissions = new Dictionary<PermissionModules, List<PermissionsSet>>
        {
            {
                PermissionModules.Common,
                new List<PermissionsSet>
                {
                   educativeorg_models.PermissionsSet.AddUsers, educativeorg_models.PermissionsSet.DeleteUsers, educativeorg_models.PermissionsSet.UpdateUsers, educativeorg_models.PermissionsSet.GetUserById, educativeorg_models.PermissionsSet.GetAllUsers
                } 
            },
            {
                PermissionModules.Accounts,
                new List<PermissionsSet>
                {
                   educativeorg_models.PermissionsSet.AddUsers, educativeorg_models.PermissionsSet.DeleteUsers, educativeorg_models.PermissionsSet.UpdateUsers, educativeorg_models.PermissionsSet.GetUserById, educativeorg_models.PermissionsSet.GetAllUsers
                }
            },


        };

   

    }
    
}
