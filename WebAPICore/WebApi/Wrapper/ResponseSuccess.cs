using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Wrapper
{
    public class ResponseSuccess<T> : IResponse
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
