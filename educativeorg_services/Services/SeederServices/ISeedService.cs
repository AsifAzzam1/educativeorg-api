﻿using educativeorg_models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.SeederServices
{
    public interface ISeedService
    {
        Task<ResponseViewModel<object>> SeedBaseData();
        Task<ResponseViewModel<object>> SeedPermissions();
    }
}
