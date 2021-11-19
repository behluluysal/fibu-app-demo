using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Wrapper
{
    public class ResponseException : Exception, IResponse
    {
        public ResponseException()
        {
        }
        public ResponseException(string[] Errors)
        {
            this.Errors = Errors;
        }

        public string[] Errors { get; set; }

        public int StatusCode { get; set; }
    }
}
