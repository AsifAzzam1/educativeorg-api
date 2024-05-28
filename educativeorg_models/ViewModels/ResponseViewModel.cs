using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels
{
    public class ResponseViewModel<T> 
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public string Message { get; set; }
        public bool Success { get; set; } = true;
        public T Data { get; set; }
    }
}
