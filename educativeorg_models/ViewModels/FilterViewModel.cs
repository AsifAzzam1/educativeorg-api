using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels
{
    public class FilterViewModel
    {
        public int PageNo { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public string? Query { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool? SortDesc { get; set; } = false;
        public bool? Status { get; set; } = null;
    }

    public class PaginateResponseModel<T>
    {
        public long TotalRecords { get; set; }
        public long RecordsAfterFilter { get; set; }
        public List<T> Data { get; set; } = new();
    }

}
