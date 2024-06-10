using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_models.Helper
{
    public static class CommonHelpersExtensions
    {
        public static string VerifyValueFrom<T>(this string data) where T : Enum
        {
            var type = typeof(T);
            var allNames = Enum.GetNames(type).ToList();
            //var name = type.Name;

            if (allNames == null || allNames.Count == 0)
                throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest,
                    "Invalid Values. No possible option are Provided for selection in " + type.Name);

            if (!allNames!.Any(x => x == data))
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest,
                "Invalid Values provided for " + type.Name);


            return data;
        }
    }
}
