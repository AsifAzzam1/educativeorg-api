using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels
{
    
        public class ExceptionResposne
        {
            public string Type { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
            public bool Status { get; set; }
            public int StatusCode { get; set; }
            public string ErrorCode { get; set; }
            public ExceptionResposne(Exception ex)

            {
                //type like argument exception
                Type = ex.GetType().Name;
                Message = ex.Message;
                Status = false;
                StackTrace = ex.ToString();
                StatusCode = 500;
                if (ex is HttpStatusException httpException)
                {
                    this.StatusCode = (int)httpException.Status;
                    this.ErrorCode = httpException.ErrorCode;
                }
            }
            public class HttpStatusException : Exception
            {
                public HttpStatusCode Status { get; set; }
                public string ErrorCode { get; set; }
                public HttpStatusException(HttpStatusCode code, string msg, string ErrorCode = null) : base(msg)
                {
                    this.Status = code;
                    this.ErrorCode = ErrorCode;
                }
            }
        }
    
}
