

namespace educativeorg_models
{
    public static class EducativeOrg_Constants
    {
        public static string JWtkey = "this is a very long long key, you can chnage it whenever you want";



        public static Dictionary<PermissionModules, List<Permissions>> Permissions = new Dictionary<PermissionModules, List<Permissions>>
        {
            {
                PermissionModules.Common,
                new List<Permissions>
                {
                   educativeorg_models.Permissions.CanAdd, educativeorg_models.Permissions.CanDelete, educativeorg_models.Permissions.CanUpdate, educativeorg_models.Permissions.CanGetById, educativeorg_models.Permissions.CanGetAll
                } 
            },
            {
                PermissionModules.Accounts,
                new List<Permissions>
                {
                   educativeorg_models.Permissions.CanAdd, educativeorg_models.Permissions.CanDelete, educativeorg_models.Permissions.CanUpdate, educativeorg_models.Permissions.CanGetById, educativeorg_models.Permissions.CanGetAll
                }
            },


        };



    }
    
}
