using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels.Company
{
    public class CompanyViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
